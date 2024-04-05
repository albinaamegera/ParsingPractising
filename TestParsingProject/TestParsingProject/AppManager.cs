using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;
using System.Globalization;

namespace TestParsingProject
{
    public class AppManager
    {
        private readonly string _url = "https://www.kaggle.com/datasets/mkechinov/ecommerce-behavior-data-from-multi-category-store";
        private ChromeDriver? _driver;
        private WebDriverWait? _waiter;
        private IWebElement? _directNode;

        public List<Product> Products { get; private set; }

        public int CurrentIndex { get; private set; }


        public AppManager()
        {
            Products = [];
            ChromeOptions options = new();
            options.AddArguments("--headless=new");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--enable-chrome-browser-cloud-management");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--ignore-ssl-errors");
            _driver = new(options);
            _waiter = new(_driver, TimeSpan.FromSeconds(20));
            _waiter.PollingInterval = TimeSpan.FromMilliseconds(200);

            _driver.Navigate().GoToUrl(_url);
        }
        private IWebElement GetNode(By locator)
        {
            return _waiter.Until(ExpectedConditions.ElementExists(locator));
        }
        public double GetSum()
        {
            int index = CurrentIndex;
            double sum = 0;
            for (int i = 0; i < index; i++)
            {
                sum += Products[i].Price;
            }
            return sum;
        }
        public string GetCategory()
        {
            int index = CurrentIndex;
            List<Product> elements = Products.GetRange(0, index);
            Dictionary<string, int> value = new()
            {
                { elements.First().Category, 1 }
            };
            for (int i = 1; i > index; i++)
            {
                if (value.ContainsKey(elements[i].Category))
                {
                    value[elements[i].Category] += 1;
                }
                else
                {
                    value.Add(elements[i].Category, 1);
                }
            }
            var max = value.Max(x => x.Value);
            return value.FirstOrDefault(x => x.Value == max).Key;
        }
        public string GetBrand()
        {
            int index = CurrentIndex;
            List<Product> elements = Products.GetRange(0, index);
            Dictionary<string, int> value = new()
            {
                { elements.First().Brand, 1 }
            };
            for (int i = 1; i > index; i++)
            {
                if (value.ContainsKey(elements[i].Brand))
                {
                    value[elements[i].Brand] += 1;
                }
                else
                {
                    value.Add(elements[i].Brand, 1);
                }
            }
            var max = value.Max(x => x.Value);
            return value.FirstOrDefault(x => x.Value == max).Key;
        }
        public string GetProductName()
        {
            int index = CurrentIndex;
            List<Product> elements = Products.GetRange(0, index);
            Dictionary<string, int> value = new()
            {
                { elements.First().Name, 1 }
            };
            for (int i = 1; i > index; i++)
            {
                if (value.ContainsKey(elements[i].Name))
                {
                    value[elements[i].Name] += 1;
                }
                else
                {
                    value.Add(elements[i].Name, 1);
                }
            }
            var max = value.Max(x => x.Value);
            return value.FirstOrDefault(x => x.Value == max).Key;
        }
        public void ReadProducts(object cnlToken)
        {

            CancellationToken token = (CancellationToken)cnlToken;
            CurrentIndex = 0;

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine($"cancel registered at index {CurrentIndex}");
                    break;
                }
                try
                {
                    _directNode = GetNode(By.XPath($"//*[@id=\"site-content\"]/div[2]/div/div[2]/div/div[5]/div[4]/div/div/div[1]/div/div[3]/div[6]/span[{++CurrentIndex}]/div"));
                    var elements = _directNode.FindElements(By.XPath($"div"));
                    AddNote(elements.ToList());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Thread.Sleep(500);
            }
            _driver.Quit();
        }
        private void AddNote(List<IWebElement> elements)
        {
            var strs = elements[4].Text.Split(".");
            var brand = elements[5].Text;
            var price = elements[6].Text;

            Products.Add(new Product
            {
                Name = strs.Last(),
                Category = strs.First(),
                Brand = brand,
                Price = (float)Convert.ToDecimal(price, new CultureInfo("en-US"))
            });
        }
    }
}
