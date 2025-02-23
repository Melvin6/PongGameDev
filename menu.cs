using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class Menu
{
    private SpriteFont font;
    private List<string> options;
    private int selectedIndex;
    private Vector2 position;
    private Color selectedColor = Color.Gray;
    private Color normalColor = Color.White;

    public bool IsVisible { get; set; } = false;

    // Callbacks for menu actions
    private Action onRestart;
    private Action onQuit;
    private Action onResume;

    // Game state
    public enum GameState { Playing, Paused, GameOver }
    public GameState CurrentState { get; set; } = GameState.Playing;

    public Menu(SpriteFont font, Vector2 position, List<string> options, Action onRestart, Action onQuit, Action onResume)
    {
        this.font = font;
        this.position = position;
        this.options = options;
        this.selectedIndex = 0;
        this.onRestart = onRestart;
        this.onQuit = onQuit;
        this.onResume = onResume;
    }

    public void Update()
    {
        if (!IsVisible) return;
        SetOptionsForState(CurrentState);

        KeyboardState keyboardState = Keyboard.GetState();
        MouseState mouseState = Mouse.GetState();

        if (keyboardState.IsKeyDown(Keys.Up)) {
            selectedIndex = (selectedIndex - 1 + options.Count) % options.Count;
        }
        if (keyboardState.IsKeyDown(Keys.Down)) {
            selectedIndex = (selectedIndex + 1) % options.Count;
        }
        if (keyboardState.IsKeyDown(Keys.Enter)) {
            SelectOption();
        }

        for (int i = 0; i < options.Count; i++) {
            Rectangle optionBounds = new Rectangle(
                (int)position.X -  (int)font.MeasureString(options[i]).X / 2,
                (int)position.Y + i * 30,
                (int)font.MeasureString(options[i]).X,
                (int)font.MeasureString(options[i]).Y
            );

            if (optionBounds.Contains(mouseState.Position)) {
                selectedIndex = i;

                if (mouseState.LeftButton == ButtonState.Pressed) {
                    SelectOption();
                }
            }
        }
    }

    private void SelectOption()
    {
        string selectedOption = options[selectedIndex];

        switch (selectedOption) {
            case "Resume":
                onResume?.Invoke();
                break;
            case "Restart":
                onRestart?.Invoke();
                break;
            case "Quit":
                onQuit?.Invoke();
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsVisible) return;

        if (CurrentState == GameState.GameOver) {
            string gameOverText = "Game Over";
            Vector2 gameOverPosition = new Vector2(
                position.X - font.MeasureString(gameOverText).X / 2,
                position.Y - 50
            );
            spriteBatch.DrawString(font, gameOverText, gameOverPosition, Color.Red);
        }
        else {
            string gameOverText = "Paused";
            Vector2 gameOverPosition = new Vector2(
                position.X - font.MeasureString(gameOverText).X / 2,
                position.Y - 50
            );
            spriteBatch.DrawString(font, gameOverText, gameOverPosition, Color.Red);
        }

        for (int i = 0; i < options.Count; i++) {
            Color color = (i == selectedIndex) ? selectedColor : normalColor;
            Vector2 gameOptions = new Vector2(
                position.X - font.MeasureString(options[i]).X / 2,
                position.Y + i * 30
            );
            
            spriteBatch.DrawString(font, options[i], gameOptions, color);
        }
    }

    public void SetOptionsForState(GameState state)
    {
        switch (state) {
            case GameState.Paused:
                options = new List<string> { "Resume", "Restart", "Quit" };
                break;
            case GameState.GameOver:
                options = new List<string> { "Restart", "Quit" };
                break;
        }
    }
}