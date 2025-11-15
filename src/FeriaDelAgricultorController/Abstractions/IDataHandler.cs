namespace FeriaDelAgricultorController.Abstractions
{
    /// <summary>
    /// Interface for data handler. The idea is to have a contract for data operations.
    /// </summary>
    public interface IDataHandler<T>
    {
        /// <summary>
        /// Saves the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// true if data is saved successfully; otherwise, false.
        /// </returns>
        public bool SaveData(List<T> data, string fileName);

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// the list of data loaded.
        /// </returns>
        public List<T> LoadData(string fileName);
    }
}
