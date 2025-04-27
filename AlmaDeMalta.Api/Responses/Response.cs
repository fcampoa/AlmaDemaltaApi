using System.Net;

namespace AlmaDeMalta.api.Responses;
public record Response(object Body, HttpStatusCode Status, string SuccessMessage, string ErrorMessage)
{
    // Clase Builder anidada para construir respuestas
    public class Builder
    {
        private object _body = new object(); // Inicialización para evitar CS8618
        private HttpStatusCode _status = HttpStatusCode.OK; // Valor por defecto
        private string _successMessage = string.Empty;
        private string _errorMessage = string.Empty;

        public Builder WithBody(object body)
        {
            _body = body;
            return this;
        }

        public Builder WithStatus(HttpStatusCode status) // Cambiar el tipo de parámetro a HttpStatusCode
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
            _status = HttpStatusCode.OK; // Usar HttpStatusCode en lugar de int
            _successMessage = message;
            _errorMessage = string.Empty;
            return this;
        }

        public Builder AsError(string message = "", HttpStatusCode status = HttpStatusCode.BadRequest) // Cambiar el tipo de parámetro a HttpStatusCode
        {
            _status = status;
            _errorMessage = message;
            _successMessage = string.Empty;
            return this;
        }

        public Builder AsNotFound(string message = "Resource not found")
        {
            return AsError(message, HttpStatusCode.NotFound); // Usar HttpStatusCode.NotFound
        }

        public Builder AsServerError(string message = "Internal server error")
        {
            return AsError(message, HttpStatusCode.InternalServerError); // Usar HttpStatusCode.InternalServerError
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
    public bool IsSuccess => (int)Status >= 200 && (int)Status < 300; // Convertir HttpStatusCode a int para la comparación

    public bool IsError => !IsSuccess;
}
