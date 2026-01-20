namespace BLL;

public class Presets (GameState gameState){

    public void Connect4Preset() => ApplyPreset(6, 6, 4, "rectangle"); 
    public void Connect5Preset() => ApplyPreset(8, 8, 5, "rectangle");
    public void CylinderPreset() => ApplyPreset(6, 6, 4, "cylinder"); 
    
    private void ApplyPreset(int boardHeight, int boardWidth, int checkersWinningSize, string boardShape ) {
        gameState.BoardHeight = boardHeight;
        gameState.BoardWidth = boardWidth;
        gameState.CheckersWinningSize = checkersWinningSize;
        gameState.BoardShape = boardShape;
        gameState.Resize();
    }
}
