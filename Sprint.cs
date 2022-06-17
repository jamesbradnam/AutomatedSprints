class Sprint
{
    public string name;
    public Attributes attributes;
    public Sprint(string name, string startDate, string finishDate)
    {
        this.name = name;
        this.attributes = new Attributes(startDate, finishDate);
    }
}