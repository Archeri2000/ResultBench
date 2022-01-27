// See https://aka.ms/new-console-template for more information

using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run<ResultBench>();
    
[MemoryDiagnoser]
public class ResultBench
{
    //TODO: Exception methods and tests
    //TODO: Multi Function tests
    public ResultBench()
    {
    }

    public int Add(int x, int y)
    {
        return x + y;
    }
    
    public IntO Add(IntO x, IntO y)
    {
        return new IntO(x.i + y.i);
    }

    [Benchmark]
    public int Baseline()
    {
        try
        {
            return Add(1, 1);
        }
        catch (Exception e)
        {
            return 0;
        }
    }
    [Benchmark]
    public int ResultStructTest1()
    {
        return new ResultStruct<int>(1).Then(x => Add(x, 1)).Match(x => x, _ => 0);
    }

    [Benchmark]
    public int ResultStruct2Test1()
    {
        return new SuccessStruct2<int>(1).Then(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public int ResultRecordTest1()
    {
        return new ResultRecord<int>(1).Then(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public int ResultTest1()
    {
        return new Success<int>(1).Then(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public int Result2Test1()
    {
        return new Success2<int>(1).Then(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public int ResultStructTest2()
    {
        return new ResultStruct<int>(1).Then2(x => Add(x, 1)).Match(x => x, _ => 0);
    }

    [Benchmark]
    public int ResultStruct2Test2()
    {
        return new SuccessStruct2<int>(1).Then2(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public int ResultRecordTest2()
    {
        return new ResultRecord<int>(1).Then2(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public int ResultTest2()
    {
        return new Success<int>(1).Then2(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public int Result2Test2()
    {
        return new Success2<int>(1).Then2(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public IntO BaselineO()
    {
        try
        {
            return Add(1, 1);
        }
        catch (Exception e)
        {
            return 0;
        }
    }
    
    [Benchmark]
    public IntO ResultStructTest1O()
    {
        return new ResultStruct<IntO>(1).Then(x => Add(x, 1)).Match(x => x, _ => 0);
    }

    [Benchmark]
    public IntO ResultStruct2Test1O()
    {
        return new SuccessStruct2<IntO>(1).Then(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public IntO ResultRecordTest1O()
    {
        return new ResultRecord<IntO>(1).Then(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public IntO ResultTest1O()
    {
        return new Success<IntO>(1).Then(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public IntO Result2Test1O()
    {
        return new Success2<IntO>(1).Then(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public IntO ResultStructTest2O()
    {
        return new ResultStruct<IntO>(1).Then2(x => Add(x, 1)).Match(x => x, _ => 0);
    }

    [Benchmark]
    public IntO ResultStruct2Test2O()
    {
        return new SuccessStruct2<IntO>(1).Then2(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public IntO ResultRecordTest2O()
    {
        return new ResultRecord<IntO>(1).Then2(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public IntO ResultTest2O()
    {
        return new Success<IntO>(1).Then2(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
    [Benchmark]
    public IntO Result2Test2O()
    {
        return new Success2<IntO>(1).Then2(x => Add(x, 1)).Match(x => x, _ => 0);
    }
    
}

public class IntO
{
    public int i;

    public IntO(int i)
    {
        this.i = i;
    }

    public static implicit operator IntO(int i) => new(i);
}


public readonly struct ResultStruct<T>
{
    private readonly bool IsSuccess;
    private readonly T? success;
    private readonly Exception? failure;
    public ResultStruct(T obj)
    {
        success = obj;
        IsSuccess = true;
        failure = null;
    }

    public ResultStruct(Exception e)
    {
        success = default(T);
        IsSuccess = false;
        failure = e;
    }

    public TOut Match<TOut>(Func<T, TOut> Success, Func<Exception, TOut> Failure)
    {
        return IsSuccess ? Success(success) : Failure(failure);
    }

    public ResultStruct<TOut> Then<TOut>(Func<T, TOut> func)
    {
        return Match(Lift(func), x => new ResultStruct<TOut>(x));
    }

    private static Func<T, ResultStruct<TOut>> Lift<TOut>(Func<T, TOut> func)
    {
        return x =>
        {
            try
            {
                var result = func(x);
                return new ResultStruct<TOut>(result);
            }
            catch (Exception e)
            {
                return new ResultStruct<TOut>(e);
            }
        };
    }

    public ResultStruct<TOut> Then2<TOut>(Func<T, TOut> func)
    {
        return IsSuccess ? Lift(func)(success) : new ResultStruct<TOut>(failure);
    }
}

public record ResultRecord<T>
{
    private readonly bool IsSuccess;
    private readonly T? success;
    private readonly Exception? failure;
    public ResultRecord(T obj)
    {
        success = obj;
        IsSuccess = true;
        failure = null;
    }

    public ResultRecord(Exception e)
    {
        success = default(T);
        IsSuccess = false;
        failure = e;
    }

    public TOut Match<TOut>(Func<T, TOut> Success, Func<Exception, TOut> Failure)
    {
        return IsSuccess ? Success(success) : Failure(failure);
    }

    public ResultRecord<TOut> Then<TOut>(Func<T, TOut> func)
    {
        return Match(Lift(func), x => new ResultRecord<TOut>(x));
    }

    private static Func<T, ResultRecord<TOut>> Lift<TOut>(Func<T, TOut> func)
    {
        return x =>
        {
            try
            {
                var result = func(x);
                return new ResultRecord<TOut>(result);
            }
            catch (Exception e)
            {
                return new ResultRecord<TOut>(e);
            }
        };
    }

    public ResultRecord<TOut> Then2<TOut>(Func<T, TOut> func)
    {
        return IsSuccess ? Lift(func)(success) : new ResultRecord<TOut>(failure);
    }
}

public abstract class Result<T>
{
    public TOut Match<TOut>(Func<T, TOut> Success, Func<Exception, TOut> Failure)
    {
        return this switch
        {
            Success<T> s => Success(s.value),
            Failure<T> e => Failure(e.value)
        };
    }

    public Result<TOut> Then<TOut>(Func<T, TOut> func)
    {
        return Match(Lift(func), x => new Failure<TOut>(x));
    }

    private static Func<T, Result<TOut>> Lift<TOut>(Func<T, TOut> func)
    {
        return x =>
        {
            try
            {
                var result = func(x);
                return new Success<TOut>(result);
            }
            catch (Exception e)
            {
                return new Failure<TOut>(e);
            }
        };
    }

    public Result<TOut> Then2<TOut>(Func<T, TOut> func)
    {
        return this switch
        {
            Success<T> s => Lift(func)(s.value),
            Failure<T> e => new Failure<TOut>(e.value)
        };
    }
}

public sealed class Success<T> : Result<T>
{
    public T value;

    public Success(T val)
    {
        value = val;
    }
}

public sealed class Failure<T> : Result<T>
{
    public Exception value;

    public Failure(Exception val)
    {
        value = val;
    }
}

public abstract class Result2<T>
{
    public abstract TOut Match<TOut>(Func<T, TOut> Success, Func<Exception, TOut> Failure);

    public abstract Result2<TOut> Then<TOut>(Func<T, TOut> func);

    public static Func<T, Result2<TOut>> Lift<TOut>(Func<T, TOut> func)
    {
        return x =>
        {
            try
            {
                var result = func(x);
                return new Success2<TOut>(result);
            }
            catch (Exception e)
            {
                return new Failure2<TOut>(e);
            }
        };
    }

    public abstract Result2<TOut> Then2<TOut>(Func<T, TOut> func);
}

public sealed class Success2<T> : Result2<T>
{
    public T value;

    public Success2(T val)
    {
        value = val;
    }
    
    public override TOut Match<TOut>(Func<T, TOut> Success, Func<Exception, TOut> Failure)
    {
        return Success(value);
    }
    
    public override Result2<TOut> Then<TOut>(Func<T, TOut> func)
    {
        return Match(Lift(func), x => new Failure2<TOut>(x));
    }
    
    public override Result2<TOut> Then2<TOut>(Func<T, TOut> func)
    {
        return Lift(func)(value);
    }
}

public sealed class Failure2<T> : Result2<T>
{
    public Exception value;

    public Failure2(Exception val)
    {
        value = val;
    }
    
    public override TOut Match<TOut>(Func<T, TOut> Success, Func<Exception, TOut> Failure)
    {
        return Failure(value);
    }
    
    public override Result2<TOut> Then<TOut>(Func<T, TOut> func)
    {
        return Match(Lift(func), x => new Failure2<TOut>(x));
    }
    
    public override Result2<TOut> Then2<TOut>(Func<T, TOut> func)
    {
        return new Failure2<TOut>(value);
    }
}

public interface ResultStruct2<T>
{
    public abstract TOut Match<TOut>(Func<T, TOut> Success, Func<Exception, TOut> Failure);

    public abstract ResultStruct2<TOut> Then<TOut>(Func<T, TOut> func);

    public static Func<T, ResultStruct2<TOut>> Lift<TOut>(Func<T, TOut> func)
    {
        return x =>
        {
            try
            {
                var result = func(x);
                return new SuccessStruct2<TOut>(result);
            }
            catch (Exception e)
            {
                return new FailureStruct2<TOut>(e);
            }
        };
    }

    public abstract ResultStruct2<TOut> Then2<TOut>(Func<T, TOut> func);
}

public readonly struct SuccessStruct2<T> : ResultStruct2<T>
{
    private readonly T value;

    public SuccessStruct2(T val)
    {
        value = val;
    }
    
    public TOut Match<TOut>(Func<T, TOut> Success, Func<Exception, TOut> Failure)
    {
        return Success(value);
    }
    
    public ResultStruct2<TOut> Then<TOut>(Func<T, TOut> func)
    {
        return Match(ResultStruct2<T>.Lift(func), x => new FailureStruct2<TOut>(x));
    }
    
    public ResultStruct2<TOut> Then2<TOut>(Func<T, TOut> func)
    {
        return ResultStruct2<T>.Lift(func)(value);
    }
}

public sealed class FailureStruct2<T> : ResultStruct2<T>
{
    public Exception value;

    public FailureStruct2(Exception val)
    {
        value = val;
    }
    
    public TOut Match<TOut>(Func<T, TOut> Success, Func<Exception, TOut> Failure)
    {
        return Failure(value);
    }
    
    public ResultStruct2<TOut> Then<TOut>(Func<T, TOut> func)
    {
        return Match(ResultStruct2<T>.Lift(func), x => new FailureStruct2<TOut>(x));
    }
    
    public ResultStruct2<TOut> Then2<TOut>(Func<T, TOut> func)
    {
        return new FailureStruct2<TOut>(value);
    }
}

//TODO: Expression Tree Result Type
public class ResultExpression
{
    private static Expression _expression;
    
}