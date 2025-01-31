namespace prelimproj.Models
{
    public class CalculatorModel
    {
             public string CurrentInput { get; set; } = "";
            public string Display { get; set; } = "0";
            public string LastOperation { get; set; } = "";
            public bool IsOperationClicked { get; set; } = false;


    }
}
