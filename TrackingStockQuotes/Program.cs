using TrackingStockQuotes;

class Program
{
    static async Task Main()
    {
        //асинхронно чтаем котировки из CSV файла
        List<Quote> quotes = await QuoteProcessor.ReadQuotesFromCsvAsync("StockPrices_Small.csv"); //await для ожидания окончания асинхронной операции

        string ticker = "MSFT";
        //извлекаем список котировок для ticker из общего списка котировок
        List<Quote> msftQuotes = QuoteProcessor.GetQuotesByTicker(quotes, ticker);

        //асинхронно рассчитываем средний объем торгов по акции ticker
        double averageVolume = await QuoteProcessor.CalculateAverageWeeklyVolumeAsync(msftQuotes);
        Console.WriteLine($"Средний объем торгов по акции {ticker} за неделю: {averageVolume}");
    }
}