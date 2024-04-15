using TrackingStockQuotes;

class Program
{
    static async Task Main()
    {

        var cancellationTokenSource = new CancellationTokenSource();// Создаем CancellationTokenSource
        var cancellationToken = cancellationTokenSource.Token;
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));// Отменяем операцию после 5 секунд

        try
        {
            //асинхронно чтаем котировки из CSV файла
            List<Quote> quotes = await QuoteProcessor.ReadQuotesFromCsvAsync("StockPrices_Small.csv", cancellationToken); //await для ожидания окончания асинхронной операции
            if (quotes == null || quotes.Count == 0)
            {
                Console.WriteLine("Ошибка: список котировок пустой или не удалось получить данные.");
                return;
            }

            Dictionary<string, List<Quote>> tickerQuotes = new Dictionary<string, List<Quote>>();
            string[] tickers = { "MSFT", "ABMD","MaRussia" };

            foreach (string ticker in tickers)
            {
                //извлекаем список котировок для ticker из общего списка котировок
                List<Quote> quotesForTicker = QuoteProcessor.GetQuotesByTicker(quotes, ticker);
                if (quotesForTicker == null || quotesForTicker.Count == 0)
                {
                    Console.WriteLine($"Ошибка: не удалось найти котировки для {ticker}.");
                    return;
                }
                tickerQuotes.Add(ticker, quotesForTicker);
            }

            foreach (var kvp in tickerQuotes)
            {
                string ticker = kvp.Key;
                List<Quote> tickerQuote = kvp.Value;
                //асинхронно рассчитываем средний объем торгов по акции ticker
                double averageVolume = await QuoteProcessor.CalculateAverageWeeklyVolumeAsync(tickerQuote, cancellationToken);
                Console.WriteLine($"Средний объем торгов по акции {ticker} за неделю: {averageVolume}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}