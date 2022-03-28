public static class Komuna
{
    public static string TraktiSerĉfrazon(string s) =>
            s
                .ToLower()
                .Replace("cx", "ĉ")
                .Replace("gx", "ĝ")
                .Replace("hx", "ĥ")
                .Replace("jx", "ĵ")
                .Replace("sx", "ŝ")
                .Replace("ux","ŭ");
}