-- This function will be called from Unity
function moveObjectTo(x, y, z)
    MoveTo("MyCube", x, y, z)
end

-- Call the function to move the object to a specific point
moveObjectTo(5, 2, -3)