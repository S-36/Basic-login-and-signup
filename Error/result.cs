namespace Login_and_Signup.Error
{
    // Clase genérica que representa éxito o falla
    public class Result<T>
    {
        // Los setters son privados — solo se asignan desde los métodos estáticos
        public bool IsSuccess { get; private set; }
        public T? Value { get; private set; }
        public string? Error { get; private set; }
        public int StatusCode { get; private set; }

        // Métodos estáticos para crear resultados — Necesario para Resultados que devuelven un valor (como el token en Login)
        public static Result<T> Success(T value) => new Result<T>
        {
            IsSuccess = true,
            Value = value,
            StatusCode = 200
        };

        public static Result<T> Failure(string error, int statusCode) => new Result<T>
        {
            IsSuccess = false,
            Error = error,
            StatusCode = statusCode
        };
    }

    // Versión sin valor de retorno — para operaciones como Register o que no tienen que devolver nada
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string? Error { get; private set; }
        public int StatusCode { get; private set; }

        public static Result Success(int StatusCode) => new Result
        {
            IsSuccess = true,
            StatusCode = StatusCode
        };

        public static Result Failure(string error, int statusCode) => new Result
        {
            IsSuccess = false,
            Error = error,
            StatusCode = statusCode
        };
    }
}