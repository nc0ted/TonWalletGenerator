using TonSdk.Contracts.Wallet;
using TonSdk.Core;
using TonSdk.Core.Crypto;

Console.WriteLine("TonWalletGenerator v1.0.0");

Console.WriteLine("Write the word(s) you need to find separated by commas. Example: major, blum, ton. You will find your wallets in documents folder.");

string wordsToFindInput = Console.ReadLine();
string[] wordsToFind = wordsToFindInput.Split(',').Select(sValue => sValue.Trim()).ToArray();

string wordsString = string.Empty;
foreach (var word in wordsToFind) 
{
    wordsString+= word+" ";
}
Console.WriteLine($"Searching for {wordsString}");
string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

if(!File.Exists(Path.Combine(docPath, "wallets.txt")))
{
    File.WriteAllText(Path.Combine(docPath, "wallets.txt"), "wallets---");
}
if(!File.Exists(Path.Combine(docPath, "wallets2.txt")))
{
    File.WriteAllText(Path.Combine(docPath, "wallets2.txt"), "wallets with words not in the end---");
}

new Thread(() => Generate())
.Start();
new Thread(() => Generate())
.Start();
new Thread(() => Generate())
.Start();
new Thread(() => Generate())
.Start();

Console.ReadKey();

async Task Generate()
{
    while(true){

        Mnemonic mnemonic = new Mnemonic();

        WalletV4Options optionsV4 = new WalletV4Options()
        {
            PublicKey = mnemonic.Keys.PublicKey
        };


        WalletV4 walletV4R2 = new WalletV4(optionsV4, 2); 
        var uqAdress = walletV4R2.Address.ToString(AddressType.Base64,new AddressStringifyOptions(false, false, false));
        Console.WriteLine($"generated adress: {uqAdress}");
       
        foreach (var wordToFind in wordsToFind) 
        {
            var str = uqAdress.Substring(uqAdress.Length - wordToFind.Length);
            string seedPhrase = string.Join(" ",mnemonic.Words);
            string[] lines = { $"adress: {uqAdress}", $"seed phrase: {seedPhrase}" };
            if(str.Contains(wordToFind))
            {
                
                Console.WriteLine($"FOUND {wordToFind}");
                File.AppendAllLines(Path.Combine(docPath, "wallets.txt"), lines);
            }   
            if(uqAdress.Contains(wordToFind))
            {
                Console.WriteLine($"FOUND {wordToFind}");
                File.AppendAllLines(Path.Combine(docPath, "wallets2.txt"), lines);
            }
        }
    }
}

