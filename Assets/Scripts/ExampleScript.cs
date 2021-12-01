using System;

public class ExampleScript
{
    private string name;

    private int age;

    public static event Func<string> EventExample;

    public static event Action CallAEvent;

    public delegate string DelegateExample();

    public static event DelegateExample del1, del2;

    public static DelegateExample del3, del4;

    public ExampleScript(string _name, int _age)
    {
        name = _name;
        age = _age;
    }

    public string ExampleMethod()
    {
        string nameAndAge = $"Name: {name} \n" + $"Age: {age}";

        return nameAndAge;
    }
}