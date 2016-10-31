using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System.IO;

namespace FaceService
{
    public class FaceService
    {
        private FaceServiceClient _fsClient = null;
        public FaceService()
        {
            _fsClient = new FaceServiceClient("7a0283940bd442b7b6e52ad5f5b03bbb");
        }

        public Person[] IdentifyTest(string tempFilePath)
        {
            var personGroupId = "1";
            bool isInitial = false;
            var personGroup = new PersonGroup();
            try
            {
                personGroup = GetPersonGroup(personGroupId);
                DeletePersonGroup(personGroupId);
                personGroup = GetPersonGroup(personGroupId);
            }
            catch (Exception ex)
            {
                var faceAPIException = (FaceAPIException)ex.InnerException;
                if (faceAPIException != null && faceAPIException.ErrorCode == "PersonGroupNotFound")
                {
                    //_fsClient.CreatePersonGroupAsync(personGroupId, "TestGroup", "it's a test group");
                    CreatePersonGroup(personGroupId, "TestGroup", "it's a test group");
                    isInitial = true;
                }
            }
            if (string.IsNullOrEmpty(personGroup.PersonGroupId))
            {
                return null;
            }

            if (isInitial)
            {
                InitializePersonGroup(personGroupId);
            }

            using (FileStream fs = new FileStream(tempFilePath, FileMode.Open))
            {
                var result = DetectFace(fs, true, true);
                return IdentifyPerson(personGroupId, result);
            }
        }

        public void CreatePersonGroup(string personGroupId, string name, string userData = null)
        {
            _fsClient.CreatePersonGroupAsync(personGroupId, name, userData);
        }

        public void InitializePersonGroup(string personGroupId)
        {
            var personIdKen = Task.Run(() => _fsClient.CreatePersonAsync(personGroupId, "lynn", "Lynn Li")).Result.PersonId;
            using (FileStream fsKen = new FileStream(@"C:\Users\lli508\Desktop\photo\me\1.jpg", FileMode.Open))
            {
                var personFaceId = Task.Run(() => _fsClient.AddPersonFaceAsync(personGroupId, personIdKen, fsKen)).Result.PersistedFaceId;
            }

            Task.Run(() => _fsClient.TrainPersonGroupAsync(personGroupId)).GetAwaiter().GetResult();
        }

        public PersonGroup GetPersonGroup(string personGroupId)
        {
            return Task.Run(() => _fsClient.GetPersonGroupAsync(personGroupId)).Result;
        }

        public void DeletePersonGroup(string personGroupId)
        {
            _fsClient.DeletePersonGroupAsync(personGroupId);
        }

        public Face[] DetectFace(FileStream fs, bool returnFaceId = true, bool returnFaceLandmarks = false)
        {
            var list = new List<FaceAttributeType>() {
                FaceAttributeType.Age, FaceAttributeType.FacialHair, FaceAttributeType.Gender,
                FaceAttributeType.Glasses, FaceAttributeType.HeadPose, FaceAttributeType.Smile };

            return Task.Run(() => _fsClient.DetectAsync(fs, returnFaceId, returnFaceLandmarks, list)).Result;
        }

        public Person[] IdentifyPerson(string personGroupId, Face[] faceArray)
        {
            var guids = faceArray.Select(x => x.FaceId).ToArray();
            if (guids.Count() == 0)
            {
                return null;
            }
            var identities = Task.Run(() => _fsClient.IdentifyAsync(personGroupId, guids)).Result;
            var persons = new List<Person>();
            foreach (var identity in identities)
            {
                persons.Add(Task.Run(() => _fsClient.GetPersonAsync(personGroupId, identity.Candidates[0].PersonId)).Result);
            }
            return persons.ToArray();
        }
    }
}
