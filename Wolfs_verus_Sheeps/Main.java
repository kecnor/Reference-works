import beadando.Farm;

public class Main {
    public static void main(String[] args) {
        Farm farm = new Farm();

        for (int i = 0; i < farm.height; i++) {
            for (int j = 0; j < farm.width; j++) {
                System.out.print(farm.fold[i][j]);
            }
            System.out.println();
        }
    }
}