using System;

namespace DelApp.Internals
{
    /// <summary>
    /// Protects the Windows system folder (and everything inside it) from deletion.
    /// <para>
    /// When <see cref="Enabled"/> is true, a <see cref="FileNDir"/> that points at the
    /// Windows directory or at any of its descendants is left untouched: an item added
    /// to the delete-list explicitly stays in the list, and a system folder reached while
    /// sweeping a parent (for example deleting <c>C:\</c>) is skipped together with its
    /// whole subtree. This prevents the app from removing the running OS files.
    /// </para>
    /// <para>
    /// The root is taken from the <c>SystemRoot</c> environment variable (falling back to
    /// the well-known folder) so relocated Windows installations are handled correctly.
    /// The trailing separator is required when matching, so a sibling such as
    /// <c>C:\Windows.old</c> is <b>not</b> treated as a system folder.
    /// </para>
    /// </summary>
    internal static class SystemFileProtector
    {
        private static readonly string s_systemRoot = NormalizeRoot(
            Environment.GetEnvironmentVariable("SystemRoot") ??
            Environment.GetFolderPath(Environment.SpecialFolder.Windows));

        /// <summary>
        /// Whether system folder protection is active. Defaults to on and is toggled from
        /// the "Protect system folder" menu item.
        /// </summary>
        public static bool Enabled { get; set; } = true;

        public static bool IsProtected(string fullPath)
        {
            if (!Enabled || string.IsNullOrEmpty(fullPath) || s_systemRoot == null)
                return false;

            int rootLength = s_systemRoot.Length;
            if (fullPath.Length < rootLength)
                return false;

            if (!fullPath.StartsWith(s_systemRoot, StringComparison.OrdinalIgnoreCase))
                return false;

            // Exact match (the Windows folder itself) or a descendant (requires the separator).
            return fullPath.Length == rootLength || fullPath[rootLength] == '\\';
        }

        private static string NormalizeRoot(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;
            return path.TrimEnd('\\');
        }
    }
}
