namespace WarGames.Business.Exceptions
{
	/// <summary>
	/// Explicit Fluency experiment. I prefer to abort early.
	/// But you have to go through control flow see it.
	///
	/// </summary>
	public static class ExceptionExtensions
	{
		public static void AbortWhen<TEx>(this string value, Func<string, bool> condition)
					where TEx : Exception
		{
			if (condition(value))
				throw Activator.CreateInstance<TEx>();
		}

		public static ThrowMe<string> AbortWith<TEx>(this string value)
					where TEx : Exception
		{
			return new ThrowMe<TEx, string>(value);
		}

		public static void AbortWith<TEx>(this ConditionWrapper conditionWrapper)
			where TEx : Exception
		{
			if (conditionWrapper.IsTrue)
				throw Activator.CreateInstance<TEx>();
		}

		public static void When<T>(this ThrowMe<T> throwMe, Func<T, bool> condition)
		{
			if (condition(throwMe.Value))
				throwMe.Trigger();
		}

		public static ConditionWrapper<T> When<T>(this T value, Func<T, bool> condition)
		{
			return new ConditionWrapper<T>(value, condition);
		}
	}

	public abstract class ConditionWrapper
	{
		public abstract bool IsTrue { get; }
	}

	public class ConditionWrapper<T> : ConditionWrapper
	{
		public ConditionWrapper(T value, Func<T, bool> condition)
		{
			Value = value;
			Condition = condition;
		}

		public Func<T, bool> Condition { get; }
		public override bool IsTrue => Condition(Value);
		public T Value { get; }
	}

	public abstract class ThrowMe<TValue>
	{
		private readonly TValue value;

		public ThrowMe(TValue value)
		{
			this.value = value;
		}

		public TValue Value => value;

		public abstract void Trigger();
	}

	public class ThrowMe<TEx, TValue> : ThrowMe<TValue>
		where TEx : Exception
	{
		private readonly TEx exception;

		public ThrowMe(TValue value) : base(value)
		{
			exception = Activator.CreateInstance<TEx>();
		}

		public override void Trigger()
		{
			throw exception;
		}
	}
}