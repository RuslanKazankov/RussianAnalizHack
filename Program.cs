using System.Text;

namespace RussianAnalizHack
{
    public class Program
    {
        private static Dictionary<char, int> symCount = new Dictionary<char, int>() {
            { ' ', 0 }, { 'о', 0 }, { 'е', 0 },{ 'а', 0 },{ 'и', 0 },{ 'н', 0 },
            { 'т', 0 },{ 'с', 0 },{ 'р', 0 },{ 'л', 0 },{ 'в', 0 },{ 'к', 0 },
            { 'м', 0 },{ 'п', 0 },{ 'д', 0 },{ 'у', 0 },{ 'я', 0 },{ 'з', 0 },
            { 'ы', 0 },{ 'г', 0 },{ 'б', 0 },{ 'ь', 0 },{ 'ч', 0 },{ 'й', 0 },
            { 'х', 0 },{ 'ж', 0 },{ 'ш', 0 },{ 'ц', 0 },{ 'ю', 0 },{ 'щ', 0 },
            { 'ф', 0 },{ 'э', 0 },{ 'ъ', 0 }
        };
        private static readonly string alf =    " оеаинтсрлвкмпдуязыгбьчйхжшцющфэъ";
        private static readonly string rusAlf = " абвгдежзийклмнопрстуфхцчшщъыьэюя";
        private static void Main(string[] args)
        {
            Console.WriteLine("Введите текст для расшифрования:");
            string? text = Console.ReadLine();
            Console.WriteLine("Введите длину блока:");
            int blockLength = int.Parse(Console.ReadLine());

            for (int i = 0; i < text?.Length; i++)
            {
                symCount[text[i]]++;
            }

            List<char> queue = symCount.OrderBy(pair => pair.Value).Reverse().ToDictionary().Keys.ToList();

            foreach (var item in symCount.OrderBy(pair => pair.Value).Reverse().ToDictionary())
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }

            StringBuilder decryptionText = new StringBuilder();

            foreach (char sym in text)
            {
                decryptionText.Append(alf[queue.IndexOf(sym)]);
            }

            string decryption = decryptionText.ToString();

            //Console.WriteLine("Расшифрованный текст (Попытка 1): " + decryption);

            List<int> snoski = [];

            for (int i = 0; i < blockLength; i++)
            {
                List<int> currentSnoski = [];
                for (int j = i; j < text.Length; j += blockLength)
                {
                    currentSnoski.Add(rusAlf.IndexOf(text[j]) - rusAlf.IndexOf(decryption[j]) >= 0 ? rusAlf.IndexOf(text[j]) - rusAlf.IndexOf(decryption[j]) : rusAlf.IndexOf(text[j]) - rusAlf.IndexOf(decryption[j]) + 33);
                }
                snoski.Add(currentSnoski.GroupBy(n => n)
                      .OrderByDescending(g => g.Count())
                      .Select(g => g.Key)
                      .First());
            }




            while (true)
            {
                List<char> decrtyption2 = new List<char>();
                for (int i = 0; i < text.Length; i++)
                {
                    int index = rusAlf.IndexOf(text[i]) - snoski[i % blockLength];
                    if (index < 0)
                    {
                        index += 33;
                    }
                    decrtyption2.Add(rusAlf[index]);
                }
                Console.WriteLine();
                Console.WriteLine(new string(decrtyption2.ToArray()));
                //Console.WriteLine(Decrypt(text, snoski.ToArray())); // ДЛЯ 3-ГО БЛОКА

                for (int i = 0; i < snoski.Count; i++)
                {
                    Console.WriteLine($"{i} смещение блока: " + snoski[i]);
                }

                Console.WriteLine("Выберите смещение для изменения:");
                int el = int.Parse(Console.ReadLine());
                snoski[el]++;
                if (snoski[el] == 33)
                {
                    snoski[el] = 0;
                }
            }
        }
    }
}
