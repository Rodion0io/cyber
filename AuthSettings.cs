namespace hospital_api;


// По поводу этого класса большие вопросы
public class AuthSettings
{
    public TimeSpan Expires { get; set; }
    public string SecretKey { get; set; }
}