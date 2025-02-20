using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class Menu
{
    private SpriteFont _font;
    private List<string> _options;
    private int _selectedIndex;
    private Vector2 _position;
    private Color _selectedColor = Color.Yellow;
    private Color _normalColor = Color.White;

    public bool IsVisible { get; set; } = false;

    // Callbacks for menu actions
    private Action _onRestart;
    private Action _onQuit;

    public Menu(SpriteFont font, Vector2 position, List<string> options, Action onRestart, Action onQuit)
    {
        _font = font;
        _position = position;
        _options = options;
        _selectedIndex = 0;
        _onRestart = onRestart;
        _onQuit = onQuit;
    }

    public void Update()
    {
        if (!IsVisible) return;

        KeyboardState state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.Up))
        {
            _selectedIndex = (_selectedIndex - 1 + _options.Count) % _options.Count;
        }
        if (state.IsKeyDown(Keys.Down))
        {
            _selectedIndex = (_selectedIndex + 1) % _options.Count;
        }
        if (state.IsKeyDown(Keys.Enter))
        {
            SelectOption();
        }
    }

    private void SelectOption()
    {
        string selectedOption = _options[_selectedIndex];

        if (selectedOption == "Restart")
        {
            _onRestart?.Invoke();
        }
        else if (selectedOption == "Quit")
        {
            _onQuit?.Invoke();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsVisible) return;

        for (int i = 0; i < _options.Count; i++)
        {
            Color color = (i == _selectedIndex) ? _selectedColor : _normalColor;
            spriteBatch.DrawString(_font, _options[i], _position + new Vector2(0, i * 30), color);
        }
    }
}
