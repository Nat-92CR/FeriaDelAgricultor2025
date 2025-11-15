namespace FeriaDelAgricultorModels
{
    /// <summary>
    /// Class in charge of representing a user direction.
    /// </summary>
    public class Direccion
    {
        /// <summary>
        /// Gets or sets the province.
        /// </summary>
        /// <value>
        /// The province.
        /// </value>
        public string Province { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the district.
        /// </summary>
        /// <value>
        /// The district.
        /// </value>
        public string District { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the other details.
        /// </summary>
        /// <value>
        /// The other details.
        /// </value>
        public string OtherDetails { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this direction is User's principal direction.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is principal; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrincipal { get; set;} = false;
    }
}
