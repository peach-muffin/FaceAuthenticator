using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FaceAuthenticator.DataAccessLayer;
using FaceAuthenticator.Model;
using FaceAuthenticator.Services;

namespace FaceAuthenticator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        const string subscriptionKey ="enter subscription key";
        const string endPoint = "endpoint to be entered here";

        private readonly IAuthDL _authDL;

        private readonly IBlobService _blobService;

        private IFaceClient faceClient;
        private IDbService _dbService;
        private MySqlConnection _mySqlConnection;
       

        public AuthController(IAuthDL authDL, IBlobService blobService, IDbService dbService)
        {
            _authDL = authDL;
            _blobService = blobService;
            faceClient = Authenticate(endPoint, subscriptionKey);

            //DeletePersonGroups(faceClient);
            //faceClient.PersonGroup.CreateAsync(Constants.PersonGroupId, Constants.PersonGroupName, recognitionModel: RecognitionModel.Recognition04).GetAwaiter().GetResult();
            //faceClient.PersonGroup.GetAsync("107d9d62-9126-44ee-a5b8-9f089f14ed62").GetAwaiter().GetResult();

            //faceClient.PersonGroup.

            _dbService = dbService;
            _mySqlConnection = _dbService.MySqlConnection;
        }

        

        private async Task DeletePersonGroups()
        {
            var listOfPersonGroupContainer = await faceClient.PersonGroup.ListAsync();
            foreach (var personGroup in listOfPersonGroupContainer)
            {
                await faceClient.PersonGroup.DeleteAsync(personGroup.PersonGroupId);
            }
        }

        private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, string url, string recognition_model)
        {
            // Detect faces from image URL. Since only recognizing, use the recognition model 1.
            // We use detection model 3 because we are not retrieving attributes.
            IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithUrlAsync(url, recognitionModel: recognition_model);
            List<DetectedFace> sufficientQualityFaces = new List<DetectedFace>();
            foreach (DetectedFace detectedFace in detectedFaces)
            {
                sufficientQualityFaces.Add(detectedFace);
            }
            Console.WriteLine($"{detectedFaces.Count} face(s) with {sufficientQualityFaces.Count} having sufficient quality for recognition detected from image `{Path.GetFileName(url)}`");

            return sufficientQualityFaces.ToList();
        }


        private static IFaceClient Authenticate(string endpoint, string subscriptionKey)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(subscriptionKey))
            {
                Endpoint = endpoint
            };
        }

       
        public IActionResult Get()
        {
            return Ok("Api running");
        }

        [Microsoft.AspNetCore.Cors.EnableCors("CorsPolicy")]
        [HttpPost]
        [Route("DeletePersonGroupContainer")]
        public async Task<IActionResult> DeletePersonGroupContainer()
        {
            await DeletePersonGroups();
            return Ok();
        }

        [Microsoft.AspNetCore.Cors.EnableCors("CorsPolicy")]
        [HttpPost]
        [Route("CreatePersonGroupContainer")]
        public async Task<IActionResult> CreatePersonGroupContainer()
        {
            await faceClient.PersonGroup.CreateAsync(Constants.PersonGroupId, Constants.PersonGroupName, recognitionModel: RecognitionModel.Recognition04);
            return Ok();
        }

        [Microsoft.AspNetCore.Cors.EnableCors("CorsPolicy")]
        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp(SignUpRequest request)
        {
            SignUpResponse response = new SignUpResponse();
            try
            {
                response = await _authDL.SignUp(request);
            }catch(Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
            }

            return Ok(response);
        }

        [Microsoft.AspNetCore.Cors.EnableCors("CorsPolicy")]
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadAsync([FromForm]IFormCollection form)
        {
            //var files = form.Files;

            //if(files.Count == 0)
            //{
            //    return BadRequest("File not uploaded");
            //}

            //var file = files[0];

            Directory.CreateDirectory("temp");
            string filePath = null;

            if (!form.ContainsKey("file"))
            {
                return BadRequest("File not uploaded");
            }
            string fileString = form["file"];
            fileString = fileString.Replace("data:image/jpeg;base64,", "");
            byte[] bytes = Convert.FromBase64String(fileString);
            filePath = $"temp\\Image-{Guid.NewGuid()}.jpg";
            string filename = Path.GetFileName(filePath);
            System.IO.File.WriteAllBytes(filePath, bytes);

            string username = form["username"];
            Console.WriteLine(username);

            if (username == null)
            {
                return BadRequest("Username cannot be null");
            }

            //_blobService.upload(file);
            _blobService.upload(filePath);

            string urlImage = $"https://kakulfacerecogstorage.blob.core.windows.net/facerecognitioncontainer/{filename}";

            string userGuidString = null;

            string SqlQueryForGuid = @$"SELECT Guid FROM crudoperation.userdetail WHERE UserName=@UserName;";

            if (_mySqlConnection.State != System.Data.ConnectionState.Open)
            {
                _mySqlConnection.Open();

            }
            using (MySqlCommand sqlCommand = new MySqlCommand(SqlQueryForGuid, _mySqlConnection))
            {
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandTimeout = 180;
                sqlCommand.Parameters.AddWithValue("@UserName", username);

                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        while (dataReader.HasRows)
                        {
                            while(dataReader.Read())
                            {
                               userGuidString = dataReader.GetString(0);
                            }
                            dataReader.NextResult();
                        }
                    }
            }
            Person person;
            if(userGuidString != null)
            {
                person = await faceClient.PersonGroupPerson.CreateAsync(Constants.PersonGroupId, username);
            }
            else
            {
                Guid persistedPersonId = Guid.Parse(userGuidString);
                person = await faceClient.PersonGroupPerson.GetAsync(Constants.PersonGroupId, persistedPersonId);
            }


            Console.WriteLine($"userGuidString: {userGuidString} personId: {person.PersonId}");
            //using (Stream stream = File.OpenRead(urlImage))
            //{
            //    await faceClient.PersonGroupPerson.AddFaceFromStreamAsync(personGroupId, personId, stream);

            //}


            //faceClient.PersonGroupPerson.AddFaceFromUrlAsync(Constants.PersonGroupId, username, urlImage);
            //FaceAttributeType[] faceAttributes =
            //{
            //     FaceAttributeType.QualityForRecognition
            //};

            //IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithUrlAsync(urlImage, recognitionModel: RecognitionModel.Recognition04, detectionModel: DetectionModel.Detection03, returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.QualityForRecognition });
            //List<DetectedFace> sufficientQualityFaces = new List<DetectedFace>();
            //foreach (DetectedFace detectedFace in detectedFaces)
            //{
            //    var faceQualityForRecognition = detectedFace.FaceAttributes.QualityForRecognition;
            //    if (faceQualityForRecognition.HasValue && (faceQualityForRecognition.Value >= QualityForRecognition.Medium))
            //    {
            //        sufficientQualityFaces.Add(detectedFace);
            //    }
            //}
            //Console.WriteLine($"{detectedFaces.Count} face(s) with {sufficientQualityFaces.Count} having sufficient quality for recognition detected from image");

            PersistedFace face = await faceClient.PersonGroupPerson.AddFaceFromUrlAsync(Constants.PersonGroupId, person.PersonId,
                        urlImage);

            Console.WriteLine($"PersonId: {person.PersonId} FaceId: {face.PersistedFaceId}");
            var u = await faceClient.PersonGroupPerson.ListAsync(Constants.PersonGroupId);
            string SqlQuery = @$"UPDATE crudoperation.userdetail SET Guid='{person.PersonId}', FaceId='{face.PersistedFaceId}' WHERE UserName=@UserName;";

            if (_mySqlConnection.State != System.Data.ConnectionState.Open)
            {
                _mySqlConnection.Open();

            }
            using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
            {
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.CommandTimeout = 180;
                sqlCommand.Parameters.AddWithValue("@UserName", username);

                int Status = await sqlCommand.ExecuteNonQueryAsync();
                if (Status <= 0)
                {
                    throw new Exception("Unable to insert person id");
                }
            }

            await faceClient.PersonGroup.TrainAsync(Constants.PersonGroupId);

            while (true)
            {
                await Task.Delay(1000);
                var trainingStatus = await faceClient.PersonGroup.GetTrainingStatusAsync(Constants.PersonGroupId);
                Console.WriteLine($"Training status: {trainingStatus.Status}.");
                if (trainingStatus.Status == TrainingStatusType.Succeeded) 
                {
                    break; 
                }
            }

            System.IO.DirectoryInfo di = new DirectoryInfo("temp");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            return Ok();
        }


        
        [Microsoft.AspNetCore.Cors.EnableCors("CorsPolicy")]
        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromForm]IFormCollection form)
        {
            
            //var files = form.Files;
            // IFormFile file = null;
            Directory.CreateDirectory("temp");
            string filePath = null;
            
            if (!form.ContainsKey("file"))
            {
                return BadRequest("File not uploaded");
            }
            string fileString = form["file"];
            fileString = fileString.Replace("data:image/jpeg;base64,", "");
            byte[] bytes = Convert.FromBase64String(fileString);
            filePath = $"temp\\Image-{Guid.NewGuid()}.jpg";
            string filename = Path.GetFileName(filePath);
            System.IO.File.WriteAllBytes(filePath, bytes);


            //string f = "";
            //string u = form["file"];
            //u.


            //file = files[0];
            //string errString = "This docment uses 3 other docments to docment the docmentation";

            //Console.WriteLine("The original string is:{0}'{1}'{0}", Environment.NewLine, errString);

            //// Correct the spelling of "document".

            //string correctString = errString.Replace("docment", "document");


            string username = form["username"];
            Console.WriteLine(username);

            if (username == null)
            {
                return BadRequest("Username cannot be null");
            }
        
            //_blobService.upload(file);
            _blobService.upload(filePath);

            string urlImage = $"https://kakulfacerecogstorage.blob.core.windows.net/facerecognitioncontainer/{filename}";

            var signInTask = _authDL.SignIn(new SignInRequest() { UserName = username });


            List<Guid> sourceFaceIds = new List<Guid>();

            List<DetectedFace> detectedFaces = await DetectFaceRecognize(faceClient, urlImage, RecognitionModel.Recognition04);


            foreach (var detectedFace in detectedFaces)
            {
                sourceFaceIds.Add(detectedFace.FaceId.Value);
            }

            var identifyResults = await faceClient.Face.IdentifyAsync(sourceFaceIds, Constants.PersonGroupId);
            string foundPersonId = null;
            foreach (var identifyResult in identifyResults)
            {
                if (identifyResult.Candidates.Count == 0)
                {
                    Console.WriteLine("No person is identified for the face");
                    continue;
                }


                foundPersonId = identifyResult.Candidates[0].PersonId.ToString();
                Console.WriteLine("Person is identified for the face" +
                    $" confidence: {identifyResult.Candidates[0].Confidence}.");
            }
      
            var signResponse = await signInTask;

            System.IO.DirectoryInfo di = new DirectoryInfo("temp");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            if (foundPersonId == signResponse.Guid)
            {
                return Ok($"Welcome {signResponse.FirstName}");
            }
            else
            {
                return BadRequest("Couldn't log you in");
            }
        }
    }
}
