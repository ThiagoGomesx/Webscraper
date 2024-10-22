using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");

        using (var driver = new ChromeDriver(options))
        {
            driver.Navigate().GoToUrl("Url-- site ");
            Console.WriteLine("Página aberta!");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            int quantidadeDeCliques = 90;

            for (int i = 0; i < quantidadeDeCliques; i++)
            {
                try
                {
                    // Mudar o contexto para o iframe
                    var iframe = wait.Until(driver => driver.FindElement(By.CssSelector("seletor do iframe"))); // Substitua pelo seletor correto do iframe
                    driver.SwitchTo().Frame(iframe);

                    // Esperar até que o botão esteja presente e clicável
                    var button = wait.Until(driver =>
                    {
                        var element = driver.FindElement(By.CssSelector("seletor do botão"));
                        return element.Displayed && element.Enabled ? element : null;
                    });

                    // Clicar no botão usando JavaScript
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript("arguments[0].click();", button);
                    Console.WriteLine($"Clique {i + 1} realizado.");

                    await Task.Delay(2000); // Espera 2 segundos

                    // Captura os resultados
                    var resultados = wait.Until(driver =>
                    {
                        var elements = driver.FindElements(By.CssSelector("#tabelalegal > table > tbody > tr"));
                        return elements.Count > 0 ? elements : null;
                    });

                    foreach (var resultado in resultados)
                    {

                        Console.WriteLine($"Resultado: {resultado.Text}");
                    }

                    // Voltar para o contexto principal
                    driver.SwitchTo().DefaultContent();
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("Elemento não estava disponível ou timeout atingido.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }

            Console.WriteLine("Todos os cliques foram realizados!");
            Console.ReadLine(); // Aguarda o usuário fechar
        }
    }
}
