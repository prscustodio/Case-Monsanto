import com.google.gson.FieldNamingPolicy;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.util.HashMap;

public class Legenda {
	private HashMap<String, String> nomesExibicao = new HashMap<>();
	
	public HashMap<String, String> getNomesExibicao() {
		return nomesExibicao;
	}
	
	public void setNomesExibicao(HashMap<String, String> nomesExibicao) {
		this.nomesExibicao = nomesExibicao;
	}
	
	public void add(String chave, String nomeExibicao) {
		this.nomesExibicao.put(chave, nomeExibicao);
	}
	
	@Override
	public String toString() {
		Gson gson = new GsonBuilder()
				.setPrettyPrinting().setFieldNamingPolicy(FieldNamingPolicy.UPPER_CAMEL_CASE)
				.create();
		
		return gson.toJson(this);
	}
}
