package beadando;

import java.util.Random;

public class Farm{
    public int width;
    public int height;
    public Object[][] fold;

    private static final char EMPTY = ' ';
    private static final char WALL = '#';
    private static final char GATE = ' ';
    private static final char Wolf = 'W';
    private static final char SHEEP = 'S';

    public Farm(){
        this.width = 14;
        this.height = 14;
        this.fold = new Object[height][width];

        CreateFarm();
    }

    private void CreateFarm() {
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                if (i == 0 || i == height - 1 || j == 0 || j == width - 1) {
                    fold[i][j] = WALL;
                } else {
                    fold[i][j] = EMPTY;
                }
            }
        }

        Random rand = new Random();
        fold[0][rand.nextInt(width-2)+1] = GATE;
        fold[height-1][rand.nextInt(width-2)+1] = GATE;
        fold[rand.nextInt(height-2)+1][0] = GATE;
        fold[rand.nextInt(height-2)+1][width-1] = GATE;
    }
}