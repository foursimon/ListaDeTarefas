namespace backend.Resultados
{
	public class Result<TValue, TError>
	{
		public bool IsSuccess { get; }
		public TValue? Value { get; }
		public TError? Error { get; }

		private Result(TValue? value)
		{
			IsSuccess = true;
			Value = value;
			Error = default;
		}
		private Result(TError? error)
		{
			IsSuccess = false;
			Value = default;
			Error = error;
		}


		public static implicit operator Result<TValue, TError>(TValue value) => 
			new Result<TValue, TError>(value);
		public static implicit operator Result<TValue, TError>(TError error) => 
			new Result<TValue, TError>(error);
	}
}
