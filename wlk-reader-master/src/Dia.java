import com.google.gson.FieldNamingPolicy;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.util.ArrayList;
import java.util.HashMap;

public class Dia {
	private HashMap<String, ArrayList<Double>> dados = new HashMap<>();
	
	public HashMap<String, ArrayList<Double>> getDados() {
		return dados;
	}
	
	public void setDados(HashMap<String, ArrayList<Double>> dados) {
		this.dados = dados;
	}
	
	public void add(String chave, ArrayList<Double> valores) {
		this.dados.put(chave, valores);
	}
	
	@Override
	public String toString() {
		Gson gson = new GsonBuilder()
				.setPrettyPrinting().setFieldNamingPolicy(FieldNamingPolicy.UPPER_CAMEL_CASE)
				.create();
		
		return gson.toJson(this);
	}
}
