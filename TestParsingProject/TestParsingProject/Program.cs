namespace TestParsingProject
{
    class Program
    {
        private static AppManager _manager = new();
        private static CancellationTokenSource _cnts = new();
        public static void Main()
        {
            Console.WriteLine("Hello :)");
            

            Thread readThePageThread = new(_manager.ReadProducts);
            readThePageThread.Start(_cnts.Token);
            
            WaitForInput();
        }
        private static void WaitForInput()
        {
            Console.WriteLine("чтобы прервать чтение с сайта нажмите 1");
            Console.WriteLine("чтобы получить сумму выручки нажмите 2");
            Console.WriteLine("чтобы посмотреть прогресс выполнения нажмите 3");
            Console.WriteLine("чтобы просмотреть самую популярную категорию нажмите 4");
            Console.WriteLine("чтобы посмотреть самый популярный бренд нажмите 5");
            Console.WriteLine("чтобы постмотреть самый популярный товар нажмите 6");
            int input = int.Parse(Console.ReadLine());

            switch(input)
            {
                case 1: _cnts.Cancel(); break;
                case 2: ShowSum(); break;
                case 3: ShowProgress(); break;
                case 4: ShowCategory(); break;
                case 5: ShowBrand(); break;
                case 6: ShowProductName(); break;
                default: break;
            }
            WaitForInput();
        }
        private static void ShowSum()
        {
            Console.WriteLine($"общая сумма выручки : {_manager.GetSum().ToString("0.00")}");
        }
        private static void ShowProgress()
        {
            Console.WriteLine($"обработано элементов : {_manager.CurrentIndex}");
        }
        private static void ShowCategory()
        {
            Console.WriteLine($"самая популярная категория : {_manager.GetCategory()}");
        }
        private static void ShowBrand()
        {
            Console.WriteLine($"самый популярный бренд : {_manager.GetBrand()}");
        }
        private static void ShowProductName()
        {
            Console.WriteLine($"самый популярный товар : {_manager.GetProductName()}");
        }
    }
}
