namespace AlmaDeMalta.api.Responses;
public record Response(object Body, int Status, string SuccessMessage, string ErrorMessage)
{
    // Clase Builder anidada para construir respuestas
    public class Builder
    {
        private object _body;
        private int _status = 200; // Valor por defecto
        private string _successMessage = string.Empty;
        private string _errorMessage = string.Empty;

        public Builder WithBody(object body)
        {
            _body = body;
            return this;
        }

        public Builder WithStatus(int status)
        {
            _status = status;
            return this;
        }

        public Builder WithSuccessMessage(string message)
        {
            _successMessage = message;
            return this;
        }

        public Builder WithErrorMessage(string message)
        {
            _errorMessage = message;
            return this;
        }

        // Métodos de utilidad para respuestas comunes
        public Builder AsSuccess(string message = "")
        {
            _status = 200;
            _successMessage = message;
            _errorMessage = string.Empty;
            return this;
        }

        public Builder AsError(string message = "", int status = 400)
        {
            _status = status;
            _errorMessage = message;
            _successMessage = string.Empty;
            return this;
        }

        public Builder AsNotFound(string message = "Resource not found")
        {
            return AsError(message, 404);
        }

        public Builder AsServerError(string message = "Internal server error")
        {
            return AsError(message, 500);
        }

        public Response Build()
        {
            return new Response(_body, _status, _successMessage, _errorMessage);
        }
    }

    // Método estático para iniciar la construcción
    public static Builder CreateBuilder()
    {
        return new Builder();
    }

    // Métodos útiles para la respuesta
    public bool IsSuccess => Status >= 200 && Status < 300;

    public bool IsError => !IsSuccess;
}
