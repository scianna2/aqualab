namespace ProtoAqua
{
    public class ModelState
    {
        public string ScenarioId { get; set; }
        public string CurrTick { get; set; }
        public string CurrSync { get; set; }

        public ModelData PrevModelData { get; set; }
        public ModelData CurrModelData { get; set; }
    }
}
