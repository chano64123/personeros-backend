using Core.Specification;

namespace API.Helpers {
    public class UtilityHelper {
        public static string ToUpperFirstLetterOfWord(string source) {
            source = source.ToLower();
            string[] words = source.Split(' ');
            string final = "";
            foreach(string item in words) {
                char[] letters = item.ToCharArray();
                letters[0] = char.ToUpper(letters[0]);
                foreach(char letter in letters) {
                    final = final + letter;
                }
                final = final + " ";
            }
            return final.Trim();
        }
    }
}
