using TrackingStockQuotes;

class Program
{
    static async Task Main()
    {
        // Создаем CancellationTokenSource и передаем его в GetRecordsAsync
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1));// Отменяем операцию после 5 секунд

        try
        {
            //асинхронно чтаем котировки из CSV файла
            List<Quote> quotes = await QuoteProcessor.ReadQuotesFromCsvAsync("StockPrices_Small.csv", cancellationToken); //await для ожидания окончания асинхронной операции
            if (quotes == null || quotes.Count == 0)
            {
                Console.WriteLine("Ошибка: список котировок пустой или не удалось получить данные.");
                return;
            }

            string ticker = "MSFT";
            //извлекаем список котировок для ticker из общего списка котировок
            List<Quote> msftQuotes = QuoteProcessor.GetQuotesByTicker(quotes, ticker);
            if (msftQuotes == null || msftQuotes.Count == 0)
            {
                Console.WriteLine($"Ошибка: не удалось найти котировки для {ticker}.");
                return;
            }

            //асинхронно рассчитываем средний объем торгов по акции ticker
            double averageVolume = await QuoteProcessor.CalculateAverageWeeklyVolumeAsync(msftQuotes, cancellationToken);
            Console.WriteLine($"Средний объем торгов по акции {ticker} за неделю: {averageVolume}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}