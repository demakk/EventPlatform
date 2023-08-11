namespace EventPlatform.Domain.Common;

public enum ErrorCode
{
    //Identity Errors 100-199
    InvalidUser = 100,
    WrongPassword = 101,
    UserDoesNotExist = 102,
    UserAlreadyExists = 103,
    
    
    
    //Application Errors 900 - 999
    ApplicationError = 900
    
}