namespace AwesomeGamingRacing.Models.Enums
{
    public class DisplayName : Attribute
    {
        public string Name { get; set; }
        public DisplayName(string Name)
        {
            this.Name = Name;
        }
    }
}
