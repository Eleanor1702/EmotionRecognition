using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionRecognition.Models {

    [Serializable]
    public class UserMissingException : Exception {
        public UserMissingException() { }

        public UserMissingException(string message) : base(message) { }

        public UserMissingException(string message, Exception innerException) : base(message, innerException) { }
    }
}
