using Microsoft.AspNetCore.Mvc;
using prelimproj.Models;
using System.Diagnostics;

namespace prelimproj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static CalculatorModel _calculator = new CalculatorModel();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_calculator); 
        }

        public IActionResult Calculate(string button)
        {
            if (char.IsDigit(button[0]) || button == ".")
            {
                if (_calculator.IsOperationClicked)
                {
                    _calculator.CurrentInput = "";
                    _calculator.IsOperationClicked = false;
                }

                _calculator.CurrentInput += button;
                _calculator.Display += button;
            }
            else if (button == "C")
            {
                _calculator = new CalculatorModel();
            }
            else if (button == "=")
            {
                try
                {
                    double result = 0;

                    if (_calculator.LastOperation == "\u221A") 
                    {
                        result = Math.Sqrt(double.Parse(_calculator.CurrentInput));
                    }
                    else if (_calculator.LastOperation == "\u00B2")  
                    {
                        result = Math.Pow(double.Parse(_calculator.CurrentInput), 2);
                    }
                    else if (_calculator.LastOperation == "%")
                    {
                        result = double.Parse(_calculator.CurrentInput) / 100;
                    }
                    else
                    {
                        var dataTable = new System.Data.DataTable();
                        result = Convert.ToDouble(dataTable.Compute(_calculator.Display, string.Empty));
                    }

                    _calculator.Display = result.ToString();
                    _calculator.CurrentInput = result.ToString(); 
                }
                catch (Exception ex)
                {
                    _calculator.Display = "Error";
                }
            }
            else if (button == "\u232B") 
            {
                if (_calculator.CurrentInput.Length > 0)
                {
                    _calculator.CurrentInput = _calculator.CurrentInput.Substring(0, _calculator.CurrentInput.Length - 1);
                    _calculator.Display = _calculator.CurrentInput;
                }
            }
            else if (button == "\u221A")  
            {
                if (!_calculator.IsOperationClicked)
                {
                    _calculator.Display += "\u221A"; 
                    _calculator.IsOperationClicked = true;  
                    _calculator.LastOperation = "\u221A";
                }
            }
            else if (button == "\u00B2")  
            {
                if (!_calculator.IsOperationClicked)
                {
                    _calculator.Display += "\u00B2"; 
                    _calculator.IsOperationClicked = true; 
                    _calculator.LastOperation = "\u00B2";
                }
            }
            else if (button == "%") 
            {
                if (!_calculator.IsOperationClicked)
                {
                    _calculator.Display += "%"; 
                    _calculator.IsOperationClicked = true; 
                    _calculator.LastOperation = "%";  
                }
            }
            else
            {
                if (!_calculator.IsOperationClicked)
                {
                    _calculator.Display += button;
                    _calculator.LastOperation = button;
                    _calculator.IsOperationClicked = true;
                }
            }

            return View("Index", _calculator);
        }


        private double EvaluateExpression(string expression)
        {
            var dataTable = new System.Data.DataTable();
            return Convert.ToDouble(dataTable.Compute(expression, string.Empty));

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
