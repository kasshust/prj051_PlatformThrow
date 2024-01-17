
namespace ForwardCompatibility {

    // ReSharper disable once PartialTypeWithSinglePart
    /// <summary>
    /// .NET 3.5 の System.Enum に不足している機能を提供するための前方互換 Enum クラス
    /// </summary>
    public partial class Enum {

        /// <summary>
        /// Enum をパースする
        /// </summary>
        /// <remarks>型制約として enum に限定できないのでソコは注意が必要</remarks>
        /// <param name="source">パース対象の文字列</param>
        /// <param name="destination">宛先の enum</param>
        /// <typeparam name="T">enum 型</typeparam>
        /// <returns>パース出来たら真</returns>
        public static bool TryParse<T>(string source, out T destination) {
            bool result = false;
            destination = default(T);
            string[] names = System.Enum.GetNames(typeof(T));
            foreach (string name in names) {
                if (name != source) {
                    continue;
                }
                destination = (T)System.Enum.Parse(typeof(T), name);
                result = true;
            }
            return result;
        }

    }

}