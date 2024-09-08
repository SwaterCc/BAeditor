namespace Hono.Scripts.Battle
{
    public interface ITableHelper
    {
        public bool LoadCSV(string csvFile);

        public TableRow GetTableRow(int id);
    }
}