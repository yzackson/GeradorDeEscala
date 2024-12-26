/* Regras
 * 
 * - são três integrantes aos sábados e domingos, mas às quartas são apenas dois
 * - deve haver uma distancia de dois dias de escala de cada componente
 * - a composição deve variar pelo menos dois componentes devem
 * - cada componente tem seus dias de restrições
 * 
 * 
 * 
 */

using GeradorDeEscala;
using System.Text.Json;

var integrantesTxt = File.ReadAllText("C:\\Users\\Isaac\\Documentos\\Dev\\IASD\\GeradorDeEscala\\GeradorDeEscala\\integrantes.json");
var integrantes = JsonSerializer.Deserialize<Integrante[]>(integrantesTxt);

var diasDeCultoTxt = File.ReadAllText("C:\\Users\\Isaac\\Documentos\\Dev\\IASD\\GeradorDeEscala\\GeradorDeEscala\\dias_de_culto.json");
var diasDeCulto = JsonSerializer.Deserialize<MesDiasDeCulto[]>(diasDeCultoTxt);

// Armazena os dados do integrante selecionado
Integrante novoIntegrante = new();

// Media de repetições de cada integrante
int mean = 0;

// Lista para armazenar a escala
List<EscalaPorData> escalaGeral = [];

for (int i = 0; i < diasDeCulto.Length; i++) // Mes
{
    for (int j = 0; j < diasDeCulto[i].Dias.Length; j++) // Dia
    {
        var escalaDia = new EscalaPorData()
        {
            Mes = diasDeCulto[i].Mes,
            Dia = diasDeCulto[i].Dias[j],
            Integrantes = []
        };
        int x = 0;
        // Quantidade de integrantes por dia de culto
        while (x < 3)
        {
            // Checar restrições de data
            do
            {
                // Selecionar integrante
                novoIntegrante = GetRandomItem(integrantes);
            } while (HasRestriction(novoIntegrante, diasDeCulto[i].Mes, diasDeCulto[i].Dias[j]));

            // Checar repetição de dias seguidos
            if(escalaGeral.Count < 1)
            {
                escalaDia.Integrantes.Add(novoIntegrante);
                x++;
            } else if (escalaDia.Integrantes.Any(x => x.Nome.Equals(novoIntegrante.Nome))
                || 
                (escalaGeral[escalaGeral.Count - 1].Integrantes.Any(x => x.Nome.Equals(novoIntegrante.Nome))))
            {
                continue;
            } else
            {
                if(IsOverGlobalMean(novoIntegrante))
                {
                    continue;
                }
                escalaDia.Integrantes.Add(novoIntegrante);
                x++;
            }
        }

        escalaGeral.Add(escalaDia);
    }
}

var escalaJson = JsonSerializer.Serialize(escalaGeral);
File.WriteAllText("C:\\Users\\Isaac\\Documentos\\Dev\\IASD\\GeradorDeEscala\\GeradorDeEscala\\escala_trimestre.json", escalaJson);


Integrante GetRandomItem(Integrante[] arr)
{
    var rdm = new Random();
    int randomNumber = rdm.Next(arr.Length);
    return arr[randomNumber];
}

bool HasRestriction(Integrante integrante, string mes, string dia)
{
    if (integrante.DatasRestritas != null && integrante.DatasRestritas.Any(d => d.Mes == mes)) {
        if (integrante.DatasRestritas.Any(d => d.Dias.Any(x => x.Equals(dia))))
            return true;
        return false;
    }

    return false;
}

// Determina a média geral de repetições de cada pessoa
bool IsOverGlobalMean(Integrante integrante)
{
    // Separa todos os integrantes
    List<Integrante> arr = [];
    foreach (var diaDeEscala in escalaGeral)
    {
        arr = [.. arr, .. diaDeEscala.Integrantes];
    }

    var result = arr.AsQueryable();
    var group = result.GroupBy(x => x.Nome);
    int maiorOcorrencia = 0;

    foreach (var item in group)
    {
        var cont = item.Count();
        if (cont > maiorOcorrencia)
        {
            maiorOcorrencia = cont;
        }
    }
    mean = maiorOcorrencia;

    var integranteDoGrupo = group.FirstOrDefault(x => x.Key.Equals(integrante.Nome));
    if (integranteDoGrupo != null)
    {
        if (integranteDoGrupo.Count() > maiorOcorrencia)
        {
            return true;
        }
    }
    return false;
}





Console.WriteLine("OK");