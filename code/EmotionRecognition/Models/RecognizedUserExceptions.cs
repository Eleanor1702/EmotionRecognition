using System;

namespace EmotionRecognition.Models {

    [Serializable]
    public class UserMissingException : Exception {
        public UserMissingException() { }

        public UserMissingException(string message) : base(message) { }

        public UserMissingException(string message, Exception innerException) : base(message, innerException) { }
    }
    
    [Serializable]
    public class MoreThanOneUserException : Exception {
        public MoreThanOneUserException() { }

        public MoreThanOneUserException(string message) : base(message) { }

        public MoreThanOneUserException(string message, Exception innerException) : base(message, innerException) { }
    }

    [Serializable]
    public class UnknownException : Exception {
        public UnknownException() { }

        public UnknownException(string message) : base(message) { }

        public UnknownException(string message, Exception innerException) : base(message, innerException) { }
    }
}
