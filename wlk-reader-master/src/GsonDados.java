import com.google.gson.FieldNamingPolicy;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.annotations.SerializedName;

import java.util.ArrayList;

public class GsonDados
{

    @SerializedName("dia")private int dia;

    @SerializedName("dados")private ArrayList<Valor> valores;

    public GsonDados(int dia, ArrayList<Valor> valores) {
        this.dia = dia;
        this.valores = valores;
    }

    public int getDia() {
        return dia;
    }

    public void setDia(int dia) {
        this.dia = dia;
    }

    public ArrayList<Valor> getValores() {
        return valores;
    }

    public void setValores(ArrayList<Valor> valores) {
        this.valores = valores;
    }

    @Override
    public String toString() {
        Gson gson = new GsonBuilder().setPrettyPrinting().setFieldNamingPolicy(FieldNamingPolicy.UPPER_CAMEL_CASE).create();
        return gson.toJson(this);
    }
}
