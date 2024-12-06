namespace UserManagementApp.Models.Dtos;

public class Result
{
    protected Result(bool isSuccess, IEnumerable<Error> errors)
    {
        if (isSuccess && errors.Any())
            throw new InvalidOperationException("Cannot be successful with one or more errors.");

        if (!isSuccess && !errors.Any())
            throw new InvalidOperationException("Cannot be unsuccessful without one or more errors.");

        IsSuccess = isSuccess;
        Errors = errors;
    }

    private bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IEnumerable<Error> Errors { get; }

    public static Result Success()
    {
        return new Result(true, Error.None);
    }

    public static Result Failure(IEnumerable<Error> errors)
    {
        return new Result(false, errors);
    }

    public static implicit operator Result(Error[] errors)
    {
        return Failure(errors);
    }
}

public class Result<TValue>(TValue? data, bool isSuccess, IEnumerable<Error> errors) : Result(isSuccess, errors)
    where TValue : class
{
    public TValue Data => data;

    public static Result<TValue> Success(TValue data)
    {
        return new Result<TValue>(data, true, Error.None);
    }

    public static Result<TValue> Failure(ICollection<Error> errors)
    {
        return new Result<TValue>(null, false, errors);
    }

    public static implicit operator Result<TValue>(TValue value)
    {
        return Success(value);
    }

    public static implicit operator TValue(Result<TValue> result)
    {
        return result.IsFailure
            ? throw new InvalidOperationException("Cannot convert a failed result to a value.")
            : result.Data;
    }

    public static implicit operator Result<TValue>(Error[] errors)
    {
        return Failure(errors);
    }
}