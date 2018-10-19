import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import data.DailyWeatherData;
import data.WeatherDataRecord;
import reader.WlkReader;
import java.io.FileWriter;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class main {
    public static void main(String[] args) throws IOException {

        final File wlkFiles = new File("/C://Users/paulo/Desktop/Monsanto/Arquivos wlk/2016-08.wlk");
//        final DateTime startDateTime = new DateTime(2015, 11, 1, 0, 0);
//        final DateTime endDateTime = new DateTime(2017, 2, 28, 23, 59);

        final WlkReader wlkReader = new WlkReader(wlkFiles);
//        wlkReader.setDateTimeStart(startDateTime);
//        wlkReader.setDateTimeEnd(endDateTime);

        final List<DailyWeatherData> dailyWeatherData = wlkReader.readData();
        ArrayList<Object> dias = new ArrayList<>();

        Legenda legenda = new Legenda();
        legenda.add("direcaoVento", "Direção Vento");
        legenda.add("velocidadeVento", "Velocidade Vento");
        legenda.add("outTemp", "Temperatura Out");
        legenda.add("inTemp", "Temperatura In");
        legenda.add("inUmidade", "Umidade In");
        legenda.add("outUmidade", "Umidade Out");
        legenda.add("pressao", "Pressão");
        legenda.add("precipitacao", "Precipitação");
        //Gson gson = new Gson();

        int qtdDias = dailyWeatherData.size();
        Gson gson = new GsonBuilder().create();

        // Qtd de dias com dados gravados
        System.out.println(dailyWeatherData.size());
        // 3º dado de pressão do 1º dia
        //System.out.println(dailyWeatherData.get(0).getWeatherDataRecords().get(3).getPressure());


        //double[][][] DadosArray=new double[dailyWeatherData.size()][8][dailyWeatherData.get(1).getWeatherDataRecords().size()];
            ArrayList<ArrayList<Object>> DadosArray = new ArrayList<>();
            for (int dia = 0; dia <dailyWeatherData.size(); dia++) {
                // Todas as pressões gravadas no 1º dia
                //System.out.println("dia - "+dias);
                int n = 0;
                ArrayList<Double> dadosDirecaoVento = new ArrayList<>();
                ArrayList<Double> dadosOutTemp =  new ArrayList<>();
                ArrayList<Double> dadosInTemp = new ArrayList<>();
                ArrayList<Double> dadosVelocidadeVento = new ArrayList<>();
                ArrayList<Double> dadosInUmidade = new ArrayList<>();
                ArrayList<Double> dadosOutUmidade = new ArrayList<>();
                ArrayList<Double> dadosPressao = new ArrayList<>();
                ArrayList<Double> dadosPrecipitacao = new ArrayList<>();


                for (WeatherDataRecord data : dailyWeatherData.get(dia).getWeatherDataRecords())
                {   //System.out.println("dado - "+n);
                    //System.out.println(dailyWeatherData.get(dias).getWeatherDataRecords().get(n).getPressure());
//                    DadosArray[dias][0][n]= dailyWeatherData.get(dias).getWeatherDataRecords().get(n).getPressure();
//                    DadosArray[dias][1][n]= dailyWeatherData.get(dias).getWeatherDataRecords().get(n).getOutTemp();
//                    DadosArray[dias][2][n]= dailyWeatherData.get(dias).getWeatherDataRecords().get(n).getInTemp();
//                    DadosArray[dias][3][n]= dailyWeatherData.get(dias).getWeatherDataRecords().get(n).getInHumidity();
//                    DadosArray[dias][4][n]= dailyWeatherData.get(dias).getWeatherDataRecords().get(n).getOutHumidity();
//                    DadosArray[dias][5][n]= dailyWeatherData.get(dias).getWeatherDataRecords().get(n).getPrecipitation();
//                    DadosArray[dias][6][n]= dailyWeatherData.get(dias).getWeatherDataRecords().get(n).getWindSpeed();
//                    DadosArray[dias][7][n]= dailyWeatherData.get(dias).getWeatherDataRecords().get(n).getWindDirection();

//                    double pressao = data.getPressure();
//                    double OutTemp = data.getOutTemp();
//                    double InTemp = data.getInTemp();
//                    double InUmidade = data.getInHumidity();
//                    double OutUmidade = data.getOutTemp();
//                    double Precipitacao = data.getPrecipitation();
//                    double DirVento = data.getWindDirection();
//                    double VelVento = data.getWindSpeed();
//                    ArrayList<Valor> valores = new ArrayList<>();
//                    valores.add(new Valor("pressao",pressao));
//                    valores.add(new Valor("outTemp",OutTemp));
//                    valores.add(new Valor("inTemp",InTemp));
//                    valores.add(new Valor("inUmidade",InUmidade));
//                    valores.add(new Valor("outUmidade",OutUmidade));
//                    valores.add(new Valor("precipitacao",Precipitacao));
//                    valores.add(new Valor("dirVento",DirVento));
//                    valores.add(new Valor("VelVento",VelVento));

                    // Esse método random data eu usei pra simular só, mas aqui vc tem que que colocar todos os dia do dia
                    // de cada propriedade
                    dadosDirecaoVento.add(data.getWindDirection());
                    dadosDirecaoVento.add(data.getWindDirection());
                    dadosOutTemp.add(data.getOutTemp());
                    dadosInTemp.add(data.getInTemp());
                    dadosVelocidadeVento.add(data.getWindSpeed());
                    dadosInUmidade.add(Double.valueOf(data.getInHumidity()));
                    dadosOutUmidade.add(Double.valueOf(data.getOutHumidity()));
                    dadosPressao.add(data.getPressure());
                    dadosPrecipitacao.add(data.getPrecipitation());

                    Dia diario = new Dia();

                    diario.add("direcaoVento", dadosDirecaoVento);
                    diario.add("velocidadeVento", dadosVelocidadeVento);
                    diario.add("outTemp", dadosOutTemp);
                    diario.add("inTemp", dadosInTemp);
                    diario.add("inUmidade", dadosInUmidade);
                    diario.add("outUmidade", dadosOutUmidade);
                    diario.add("pressao", dadosPressao);
                    diario.add("precipitacao", dadosPrecipitacao);

                    dias.add(diario);
//                    GsonDados dado = new GsonDados(dia, valores);
//                    DadosArray.add(dado);
                    //n++;
                }
                DadosArray.add(dias);
            }
        //System.out.println(new Gson().toJson(DadosArray));
        //System.out.println(Arrays.deepToString(DadosArray));
        //gson.toJson(DadosArray);
//        try{
//            FileWriter writeFile = new FileWriter("saida.json");
//            //Escreve no arquivo conteudo do Objeto JSON
//            writeFile.write(new Gson().toJson(DadosArray));
//            writeFile.close();
//        }
//        catch(IOException e){
//            e.printStackTrace();
//        }
        System.out.println(gson.toJson(DadosArray));
    }

}

