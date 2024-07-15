using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    [DataContract]
    public class UploadDTO
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string TypeContent { get; set; }

        [DataMember]
        public byte[] File { get; set; }

        public UploadDTO(byte[] file)
        {
            File = file;
        }

        public UploadDTO(string fileName, string typeContent, byte[] file)
        {
            FileName = fileName;
            TypeContent = typeContent;
            File = file;
        }
        public UploadDTO() { }

    }
}
