using UnityEngine;
using MoonSharp.Interpreter;

public class LuaController : MonoBehaviour
{
    public GameObject myCube;

    void Start()
    {
        // Set up MoonSharp
        Script script = new Script();

        // Register a C# function into Lua
        script.Globals["MoveTo"] = (System.Action<string, float, float, float>)((name, x, y, z) =>
        {
            if (name == myCube.name)
            {
                myCube.transform.position = new Vector3(x, y, z);
                Debug.Log($"Moved {name} to ({x}, {y}, {z})");
            }
        });

        // Run Lua code
        string luaCode = @"
            function moveObjectTo(x, y, z)
                MoveTo('MyCube', x, y, z)
            end

            moveObjectTo(5, 2, -3)
        ";

        script.DoString(luaCode);
    }
}
