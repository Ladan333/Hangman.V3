using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.V3;
internal class GameInfo
{
	private string? _name;
	public string? Name
	{
		get { return _name; }
		set { _name = value; }
	}
	public int Lives { get; set; } = 6;
	public int Wrongs { get; set; } 
    public string? Word { get; set; }
    public char[]? HiddenWord { get; set; }
    public char[]? VisualWord { get; set; }
	public List<char> GuessedLetters { get; set; } = new List<char>();
	public bool GameOver { get; set; } = false;
	public bool Win { get; set; } = false;
	public bool Loss { get; set; } = false;
}
