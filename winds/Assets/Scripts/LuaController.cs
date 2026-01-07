using UnityEngine;
using MoonSharp.Interpreter;

public class LuaController : MonoBehaviour
{
    public GameObject targetObject;

    void Start()
    {
        // Register C# function to Lua
        Script script = new Script();

        script.Globals["MoveTo"] = (System.Action<string, double, double, double>)((name, x, y, z) =>
        {
            if (targetObject != null && targetObject.name == name)
            {
                Vector3 newPos = new Vector3((float)x, (float)y, (float)z);
                targetObject.transform.position = newPos;
                Debug.Log($"Moved '{name}' to {newPos}");
            }
            else
            {
                Debug.LogWarning($"Object '{name}' not found.");
            }
        });

        // Lua script as a string
        string luaCode = @"
            function moveObjectTo(x, y, z)
                MoveTo('Ship', x, y, z)
            end

            moveObjectTo(5, 2, -3)
        ";

        script.DoString(luaCode);
    }
}
