// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
namespace System.Data.Entity.Spatial
{
    using System.Data.Entity.Resources;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.Serialization;

    [DataContract]
    [Serializable]
    public class DbGeometry
    {
        private DbSpatialServices _spatialProvider;
        private object _providerValue;

        internal DbGeometry()
        {
        }

        internal DbGeometry(DbSpatialServices spatialServices, object spatialProviderValue)
        {
            Contract.Requires(spatialServices != null);
            Contract.Requires(spatialProviderValue != null);

            _spatialProvider = spatialServices;
            _providerValue = spatialProviderValue;
        }

        /// <summary>
        /// Gets the default coordinate system id (SRID) for geometry values.
        /// </summary>
        public static int DefaultCoordinateSystemId
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets a representation of this DbGeometry value that is specific to the underlying provider that constructed it.
        /// </summary>
        public object ProviderValue
        {
            get { return _providerValue; }
        }

        /// <summary>
        /// Gets the spatial provider that will be used for operations on this spatial type.
        /// </summary>
        public virtual DbSpatialServices Provider
        {
            get { return _spatialProvider; }
        }

        /// <summary>
        /// Gets or sets a data contract serializable well known representation of this DbGeometry value.
        /// </summary>
        [DataMember(Name = "Geometry")]
        public DbGeometryWellKnownValue WellKnownValue
        {
            get { return _spatialProvider.CreateWellKnownValue(this); }
            set
            {
                if (_spatialProvider != null)
                {
                    throw new InvalidOperationException(Strings.Spatial_WellKnownValueSerializationPropertyNotDirectlySettable);
                }

                var resolvedServices = DbSpatialServices.Default;
                _providerValue = resolvedServices.CreateProviderValue(value);
                _spatialProvider = resolvedServices;
            }
        }

        #region Well Known Binary Static Constructors

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> value based on the specified well known binary value. 
        /// </summary>
        /// <param name="wellKnownBinary">A byte array that contains a well known binary representation of the geometry value.</param>
        /// <returns>A new DbGeometry value as defined by the well known binary value with the default geometry coordinate system identifier (<see cref="DbGeometry.DefaultCoordinateSystemId"/>).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="wellKnownBinary"/> is null.</exception>
        public static DbGeometry FromBinary(byte[] wellKnownBinary)
        {
            Contract.Requires(wellKnownBinary != null);
            return DbSpatialServices.Default.GeometryFromBinary(wellKnownBinary);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> value based on the specified well known binary value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="wellKnownBinary">A byte array that contains a well known binary representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known binary value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="wellKnownBinary"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        public static DbGeometry FromBinary(byte[] wellKnownBinary, int coordinateSystemId)
        {
            Contract.Requires(wellKnownBinary != null);
            return DbSpatialServices.Default.GeometryFromBinary(wellKnownBinary, coordinateSystemId);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> line value based on the specified well known binary value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="lineWellKnownBinary">A byte array that contains a well known binary representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known binary value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lineWellKnownBinary"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        public static DbGeometry LineFromBinary(byte[] lineWellKnownBinary, int coordinateSystemId)
        {
            Contract.Requires(lineWellKnownBinary != null);
            return DbSpatialServices.Default.GeometryLineFromBinary(lineWellKnownBinary, coordinateSystemId);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> point value based on the specified well known binary value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="pointWellKnownBinary">A byte array that contains a well known binary representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known binary value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="pointWellKnownBinary"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        public static DbGeometry PointFromBinary(byte[] pointWellKnownBinary, int coordinateSystemId)
        {
            Contract.Requires(pointWellKnownBinary != null);
            return DbSpatialServices.Default.GeometryPointFromBinary(pointWellKnownBinary, coordinateSystemId);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> polygon value based on the specified well known binary value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="polygonWellKnownBinary">A byte array that contains a well known binary representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known binary value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="polygonWellKnownBinary"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        public static DbGeometry PolygonFromBinary(byte[] polygonWellKnownBinary, int coordinateSystemId)
        {
            Contract.Requires(polygonWellKnownBinary != null);
            return DbSpatialServices.Default.GeometryPolygonFromBinary(polygonWellKnownBinary, coordinateSystemId);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> multi-line value based on the specified well known binary value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="multiLineWellKnownBinary">A byte array that contains a well known binary representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known binary value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="multiLineWellKnownBinary"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "MultiLine",
            Justification = "Match OGC, EDM")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi",
            Justification = "Match OGC, EDM")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "multiLine",
            Justification = "Match OGC, EDM")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "multi",
            Justification = "Match OGC, EDM")]
        public static DbGeometry MultiLineFromBinary(byte[] multiLineWellKnownBinary, int coordinateSystemId)
        {
            Contract.Requires(multiLineWellKnownBinary != null);
            return DbSpatialServices.Default.GeometryMultiLineFromBinary(multiLineWellKnownBinary, coordinateSystemId);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> multi-point value based on the specified well known binary value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="multiPointWellKnownBinary">A byte array that contains a well known binary representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known binary value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="multiPointWellKnownBinary"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "MultiPoint",
            Justification = "Match OGC, EDM")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi",
            Justification = "Match OGC, EDM")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "multiPoint",
            Justification = "Match OGC, EDM")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "multi",
            Justification = "Match OGC, EDM")]
        public static DbGeometry MultiPointFromBinary(byte[] multiPointWellKnownBinary, int coordinateSystemId)
        {
            Contract.Requires(multiPointWellKnownBinary != null);
            return DbSpatialServices.Default.GeometryMultiPointFromBinary(multiPointWellKnownBinary, coordinateSystemId);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> multi-polygon value based on the specified well known binary value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="multiPolygonWellKnownBinary">A byte array that contains a well known binary representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known binary value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="multiPolygonWellKnownBinary"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi",
            Justification = "Match OGC, EDM")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "multi",
            Justification = "Match OGC, EDM")]
        public static DbGeometry MultiPolygonFromBinary(byte[] multiPolygonWellKnownBinary, int coordinateSystemId)
        {
            Contract.Requires(multiPolygonWellKnownBinary != null);
            return DbSpatialServices.Default.GeometryMultiPolygonFromBinary(multiPolygonWellKnownBinary, coordinateSystemId);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> collection value based on the specified well known binary value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="geometryCollectionWellKnownBinary">A byte array that contains a well known binary representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known binary value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="geometryCollectionWellKnownBinary"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        public static DbGeometry GeometryCollectionFromBinary(byte[] geometryCollectionWellKnownBinary, int coordinateSystemId)
        {
            Contract.Requires(geometryCollectionWellKnownBinary != null);
            return DbSpatialServices.Default.GeometryCollectionFromBinary(geometryCollectionWellKnownBinary, coordinateSystemId);
        }

        #endregion

        #region GML Static Constructors

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> value based on the specified Geography Markup Language (GML) value.
        /// </summary>
        /// <param name="geometryMarkup">A string that contains a Geography Markup Language (GML) representation of the geometry value.</param>
        /// <returns>A new DbGeometry value as defined by the GML value with the default geometry coordinate system identifier (SRID) (<see cref="DbGeometry.DefaultCoordinateSystemId"/>).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="geometryMarkup"/> is null.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gml")]
        public static DbGeometry FromGml(string geometryMarkup)
        {
            Contract.Requires(geometryMarkup != null);
            return DbSpatialServices.Default.GeometryFromGml(geometryMarkup);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> value based on the specified Geography Markup Language (GML) value and coordinate system identifier (SRID).
        /// </summary>
        /// <param name="geometryMarkup">A string that contains a Geography Markup Language (GML) representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the GML value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="geometryMarkup"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gml")]
        public static DbGeometry FromGml(string geometryMarkup, int coordinateSystemId)
        {
            Contract.Requires(geometryMarkup != null);
            return DbSpatialServices.Default.GeometryFromGml(geometryMarkup, coordinateSystemId);
        }

        #endregion

        #region Well Known Text Static Constructors

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> value based on the specified well known text value. 
        /// </summary>
        /// <param name="wellKnownText">A string that contains a well known text representation of the geometry value.</param>
        /// <returns>A new DbGeometry value as defined by the well known text value with the default geometry coordinate system identifier (SRID) (<see cref="DbGeometry.DefaultCoordinateSystemId"/>).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="wellKnownText"/> is null.</exception>
        public static DbGeometry FromText(string wellKnownText)
        {
            Contract.Requires(wellKnownText != null);
            return DbSpatialServices.Default.GeometryFromText(wellKnownText);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> value based on the specified well known text value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="wellKnownText">A string that contains a well known text representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known text value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="wellKnownText"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        public static DbGeometry FromText(string wellKnownText, int coordinateSystemId)
        {
            Contract.Requires(wellKnownText != null);
            return DbSpatialServices.Default.GeometryFromText(wellKnownText, coordinateSystemId);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> line value based on the specified well known text value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="lineWellKnownText">A string that contains a well known text representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known text value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="lineWellKnownText"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        public static DbGeometry LineFromText(string lineWellKnownText, int coordinateSystemId)
        {
            Contract.Requires(lineWellKnownText != null);
            return DbSpatialServices.Default.GeometryLineFromText(lineWellKnownText, coordinateSystemId);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> point value based on the specified well known text value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="pointWellKnownText">A string that contains a well known text representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known text value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="pointWellKnownText"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        public static DbGeometry PointFromText(string pointWellKnownText, int coordinateSystemId)
        {
            Contract.Requires(pointWellKnownText != null);
            return DbSpatialServices.Default.GeometryPointFromText(pointWellKnownText, coordinateSystemId);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> polygon value based on the specified well known text value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="polygonWellKnownText">A string that contains a well known text representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known text value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="polygonWellKnownText"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        public static DbGeometry PolygonFromText(string polygonWellKnownText, int coordinateSystemId)
        {
            Contract.Requires(polygonWellKnownText != null);
            return DbSpatialServices.Default.GeometryPolygonFromText(polygonWellKnownText, coordinateSystemId);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> multi-line value based on the specified well known text value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="multiLineWellKnownText">A string that contains a well known text representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known text value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="multiLineWellKnownText"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "MultiLine",
            Justification = "Match OGC, EDM")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi",
            Justification = "Match OGC, EDM")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "multiLine",
            Justification = "Match OGC, EDM")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "multi",
            Justification = "Match OGC, EDM")]
        public static DbGeometry MultiLineFromText(string multiLineWellKnownText, int coordinateSystemId)
        {
            Contract.Requires(multiLineWellKnownText != null);
            return DbSpatialServices.Default.GeometryMultiLineFromText(multiLineWellKnownText, coordinateSystemId);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> multi-point value based on the specified well known text value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="multiPointWellKnownText">A string that contains a well known text representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known text value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="multiPointWellKnownText"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "MultiPoint",
            Justification = "Match OGC, EDM")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi",
            Justification = "Match OGC, EDM")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "multiPoint",
            Justification = "Match OGC, EDM")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "multi",
            Justification = "Match OGC, EDM")]
        public static DbGeometry MultiPointFromText(string multiPointWellKnownText, int coordinateSystemId)
        {
            Contract.Requires(multiPointWellKnownText != null);
            return DbSpatialServices.Default.GeometryMultiPointFromText(multiPointWellKnownText, coordinateSystemId);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> multi-polygon value based on the specified well known text value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="multiPolygonWellKnownText">A string that contains a well known text representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known text value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="multiPolygonWellKnownText"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi",
            Justification = "Match OGC, EDM")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "multi",
            Justification = "Match OGC, EDM")]
        public static DbGeometry MultiPolygonFromText(string multiPolygonWellKnownText, int coordinateSystemId)
        {
            Contract.Requires(multiPolygonWellKnownText != null);
            return DbSpatialServices.Default.GeometryMultiPolygonFromText(multiPolygonWellKnownText, coordinateSystemId);
        }

        /// <summary>
        /// Creates a new <see cref="DbGeometry"/> collection value based on the specified well known text value and coordinate system identifier (SRID). 
        /// </summary>
        /// <param name="geometryCollectionWellKnownText">A string that contains a well known text representation of the geometry value.</param>
        /// <param name="coordinateSystemId">The identifier of the coordinate system that the new DbGeometry value should use.</param>
        /// <returns>A new DbGeometry value as defined by the well known text value with the specified coordinate system identifier.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="geometryCollectionWellKnownText"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="coordinateSystemId"/> is not valid.</exception>
        public static DbGeometry GeometryCollectionFromText(string geometryCollectionWellKnownText, int coordinateSystemId)
        {
            Contract.Requires(geometryCollectionWellKnownText != null);
            return DbSpatialServices.Default.GeometryCollectionFromText(geometryCollectionWellKnownText, coordinateSystemId);
        }

        #endregion

        #region Geometry Instance Properties

        /// </summary>
        /// Gets the coordinate system identifier (SRID) of the coordinate system used by this DbGeometry value.
        /// </summary>
        public int CoordinateSystemId
        {
            get { return _spatialProvider.GetCoordinateSystemId(this); }
        }

        /// </summary>
        /// Gets the boundary of this DbGeometry value.
        /// </summary>
        public DbGeometry Boundary
        {
            get { return _spatialProvider.GetBoundary(this); }
        }

        /// <summary>
        /// Gets the dimension of the given <see cref="DbGeometry"/> value or, if the value is a collection, the dimension of its largest element.
        /// </summary>
        public int Dimension
        {
            get { return _spatialProvider.GetDimension(this); }
        }

        /// <summary>
        /// Gets the envelope (minimum bounding box) of this DbGeometry value, as a geometry value.
        /// </summary>
        public DbGeometry Envelope
        {
            get { return _spatialProvider.GetEnvelope(this); }
        }

        /// </summary>
        /// Gets the spatial type name, as a string, of this DbGeometry value.
        /// </summary>
        public string SpatialTypeName
        {
            get { return _spatialProvider.GetSpatialTypeName(this); }
        }

        /// </summary>
        /// Gets a Boolean value indicating whether this DbGeometry value represents the empty geometry.
        /// </summary>
        public bool IsEmpty
        {
            get { return _spatialProvider.GetIsEmpty(this); }
        }

        /// </summary>
        /// Gets a Boolean value indicating whether this DbGeometry is simple.
        /// </summary>
        public bool IsSimple
        {
            get { return _spatialProvider.GetIsSimple(this); }
        }

        /// </summary>
        /// Gets a Boolean value indicating whether this DbGeometry value is considered valid.
        /// </summary>
        public bool IsValid
        {
            get { return _spatialProvider.GetIsValid(this); }
        }

        #endregion

        #region Geometry Well Known Format Conversion

        /// <summary>
        /// Generates the well known text representation of this DbGeometry value.  Includes only X and Y coordinates for points.
        /// </summary>
        /// <returns>A string containing the well known text representation of this DbGeometry value.</returns>
        public virtual string AsText()
        {
            return _spatialProvider.AsText(this);
        }

        /// <summary>
        /// Generates the well known text representation of this DbGeometry value.  Includes X coordinate, Y coordinate, Elevation (Z) and Measure (M) for points.
        /// </summary>
        /// <returns>A string containing the well known text representation of this DbGeometry value.</returns>
        internal string AsTextIncludingElevationAndMeasure()
        {
            return _spatialProvider.AsTextIncludingElevationAndMeasure(this);
        }

        /// <summary>
        /// Generates the well known binary representation of this DbGeometry value.
        /// </summary>
        /// <returns>A byte array containing the well known binary representation of this DbGeometry value.</returns>
        public byte[] AsBinary()
        {
            return _spatialProvider.AsBinary(this);
        }

        // Non-OGC
        /// <summary>
        /// Generates the Geography Markup Language (GML) representation of this DbGeometry value.
        /// </summary>
        /// <returns>A string containing the GML representation of this DbGeometry value.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gml")]
        public string AsGml()
        {
            return _spatialProvider.AsGml(this);
        }

        #endregion

        #region Geometry Operations - Spatial Relation

        /// <summary>
        /// Determines whether this DbGeometry is spatially equal to the specified DbGeometry argument.
        /// </summary>
        /// <param name="other">The geometry value that should be compared with this geometry value for equality.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is spatially equal to this geometry value; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public bool SpatialEquals(DbGeometry other)
        {
            Contract.Requires(other != null);
            return _spatialProvider.SpatialEquals(this, other);
        }

        /// <summary>
        /// Determines whether this DbGeometry is spatially disjoint from the specified DbGeometry argument.
        /// </summary>
        /// <param name="other">The geometry value that should be compared with this geometry value for disjointness.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is disjoint from this geometry value; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public bool Disjoint(DbGeometry other)
        {
            Contract.Requires(other != null);
            return _spatialProvider.Disjoint(this, other);
        }

        /// <summary>
        /// Determines whether this DbGeometry value spatially intersects the specified DbGeometry argument.
        /// </summary>
        /// <param name="other">The geometry value that should be compared with this geometry value for intersection.</param>
        /// <returns><c>true</c> if <paramref name="other"/> intersects this geometry value; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public bool Intersects(DbGeometry other)
        {
            Contract.Requires(other != null);
            return _spatialProvider.Intersects(this, other);
        }

        /// <summary>
        /// Determines whether this DbGeometry value spatially touches the specified DbGeometry argument.
        /// </summary>
        /// <param name="other">The geometry value that should be compared with this geometry value.</param>
        /// <returns><c>true</c> if <paramref name="other"/> touches this geometry value; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public bool Touches(DbGeometry other)
        {
            Contract.Requires(other != null);
            return _spatialProvider.Touches(this, other);
        }

        /// <summary>
        /// Determines whether this DbGeometry value spatially crosses the specified DbGeometry argument.
        /// </summary>
        /// <param name="other">The geometry value that should be compared with this geometry value.</param>
        /// <returns><c>true</c> if <paramref name="other"/> crosses this geometry value; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public bool Crosses(DbGeometry other)
        {
            Contract.Requires(other != null);
            return _spatialProvider.Crosses(this, other);
        }

        /// <summary>
        /// Determines whether this DbGeometry value is spatially within the specified DbGeometry argument.
        /// </summary>
        /// <param name="other">The geometry value that should be compared with this geometry value for containment.</param>
        /// <returns><c>true</c> if this geometry value is within <paramref name="other"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public bool Within(DbGeometry other)
        {
            Contract.Requires(other != null);
            return _spatialProvider.Within(this, other);
        }

        /// <summary>
        /// Determines whether this DbGeometry value spatially contains the specified DbGeometry argument.
        /// </summary>
        /// <param name="other">The geometry value that should be compared with this geometry value for containment.</param>
        /// <returns><c>true</c> if this geometry value contains <paramref name="other"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public bool Contains(DbGeometry other)
        {
            Contract.Requires(other != null);
            return _spatialProvider.Contains(this, other);
        }

        /// <summary>
        /// Determines whether this DbGeometry value spatially overlaps the specified DbGeometry argument.
        /// </summary>
        /// <param name="other">The geometry value that should be compared with this geometry value for overlap.</param>
        /// <returns><c>true</c> if this geometry value overlaps <paramref name="other"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public bool Overlaps(DbGeometry other)
        {
            Contract.Requires(other != null);
            return _spatialProvider.Overlaps(this, other);
        }

        /// <summary>
        /// Determines whether this DbGeometry value spatially relates to the specified DbGeometry argument according to the
        /// given Dimensionally Extended Nine-Intersection Model (DE-9IM) intersection pattern.
        /// </summary>
        /// <param name="other">The geometry value that should be compared with this geometry value for relation.</param>
        /// <param name="matrix">A string that contains the text representation of the (DE-9IM) intersection pattern that defines the relation.</param>
        /// <returns><c>true</c> if this geometry value relates to <paramref name="other"/> according to the specified intersection pattern matrix; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> or <paramref name="matrix"/> is null.</exception>
        public bool Relate(DbGeometry other, string matrix)
        {
            Contract.Requires(other != null);
            Contract.Requires(matrix != null);
            return _spatialProvider.Relate(this, other, matrix);
        }

        #endregion

        #region Geometry Operations - Spatial Analysis

        /// <summary>
        /// Creates a geometry value representing all points less than or equal to <paramref name="distance"/> from this DbGeometry value.
        /// </summary>
        /// <param name="distance">A double value specifying how far from this geometry value to buffer.</param>
        /// <returns>A new DbGeometry value representing all points less than or equal to <paramref name="distance"/> from this geometry value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="distance"/> is null.</exception>
        public DbGeometry Buffer(double? distance)
        {
            if (!distance.HasValue)
            {
                throw new ArgumentNullException("distance");
            }
            return _spatialProvider.Buffer(this, distance.Value);
        }

        /// <summary>
        /// Computes the distance between the closest points in this DbGeometry value and another DbGeometry value.
        /// </summary>
        /// <param name="other">The geometry value for which the distance from this value should be computed.</param>
        /// <returns>A double value that specifies the distance between the two closest points in this geometry value and <paramref name="other"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public double? Distance(DbGeometry other)
        {
            Contract.Requires(other != null);
            return _spatialProvider.Distance(this, other);
        }

        /// <summary>
        /// Gets the convex hull of this DbGeometry value as another DbGeometry value.
        /// </summary>
        public DbGeometry ConvexHull
        {
            get { return _spatialProvider.GetConvexHull(this); }
        }

        /// <summary>
        /// Computes the intersection of this DbGeometry value and another DbGeometry value.
        /// </summary>
        /// <param name="other">The geometry value for which the intersection with this value should be computed.</param>
        /// <returns>A new DbGeometry value representing the intersection between this geometry value and <paramref name="other"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public DbGeometry Intersection(DbGeometry other)
        {
            Contract.Requires(other != null);
            return _spatialProvider.Intersection(this, other);
        }

        /// <summary>
        /// Computes the union of this DbGeometry value and another DbGeometry value.
        /// </summary>
        /// <param name="other">The geometry value for which the union with this value should be computed.</param>
        /// <returns>A new DbGeometry value representing the union between this geometry value and <paramref name="other"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public DbGeometry Union(DbGeometry other)
        {
            Contract.Requires(other != null);
            return _spatialProvider.Union(this, other);
        }

        /// <summary>
        /// Computes the difference between this DbGeometry value and another DbGeometry value.
        /// </summary>
        /// <param name="other">The geometry value for which the difference with this value should be computed.</param>
        /// <returns>A new DbGeometry value representing the difference between this geometry value and <paramref name="other"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public DbGeometry Difference(DbGeometry other)
        {
            Contract.Requires(other != null);
            return _spatialProvider.Difference(this, other);
        }

        /// <summary>
        /// Computes the symmetric difference between this DbGeometry value and another DbGeometry value.
        /// </summary>
        /// <param name="other">The geometry value for which the symmetric difference with this value should be computed.</param>
        /// <returns>A new DbGeometry value representing the symmetric difference between this geometry value and <paramref name="other"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public DbGeometry SymmetricDifference(DbGeometry other)
        {
            Contract.Requires(other != null);
            return _spatialProvider.SymmetricDifference(this, other);
        }

        #endregion

        #region Geometry Collection

        /// <summary>
        /// Gets the number of elements in this DbGeometry value, if it represents a geometry collection.
        /// <returns>The number of elements in this geometry value, if it represents a collection of other geometry values; otherwise <c>null</c>.</returns>
        /// </summary>
        public int? ElementCount
        {
            get { return _spatialProvider.GetElementCount(this); }
        }

        /// <summary>
        /// Returns an element of this DbGeometry value from a specific position, if it represents a geometry collection.
        /// <param name="index">The position within this geometry value from which the element should be taken.</param>
        /// <returns>The element in this geometry value at the specified position, if it represents a collection of other geometry values; otherwise <c>null</c>.</returns>
        /// </summary>
        public DbGeometry ElementAt(int index)
        {
            return _spatialProvider.ElementAt(this, index);
        }

        #endregion

        #region Point

        /// <summary>
        /// Gets the X coordinate of this DbGeometry value, if it represents a point.
        /// <returns>The X coordinate value of this geometry value, if it represents a point; otherwise <c>null</c>.</returns>
        /// </summary>
        public double? XCoordinate
        {
            get { return _spatialProvider.GetXCoordinate(this); }
        }

        /// <summary>
        /// Gets the Y coordinate of this DbGeometry value, if it represents a point.
        /// <returns>The Y coordinate value of this geometry value, if it represents a point; otherwise <c>null</c>.</returns>
        /// </summary>
        public double? YCoordinate
        {
            get { return _spatialProvider.GetYCoordinate(this); }
        }

        /// <summary>
        /// Gets the elevation (Z coordinate) of this DbGeometry value, if it represents a point.
        /// <returns>The elevation (Z coordinate) of this geometry value, if it represents a point; otherwise <c>null</c>.</returns>
        /// </summary>
        public double? Elevation
        {
            get { return _spatialProvider.GetElevation(this); }
        }

        /// <summary>
        /// Gets the Measure (M coordinate) of this DbGeometry value, if it represents a point.
        /// <returns>The Measure (M coordinate) value of this geometry value, if it represents a point; otherwise <c>null</c>.</returns>
        /// </summary>
        public double? Measure
        {
            get { return _spatialProvider.GetMeasure(this); }
        }

        #endregion

        #region Curve

        /// <summary>
        /// Gets a nullable double value that indicates the length of this DbGeometry value, which may be null if this value does not represent a curve.
        /// </summary>
        public double? Length
        {
            get { return _spatialProvider.GetLength(this); }
        }

        /// <summary>
        /// Gets a DbGeometry value representing the start point of this value, which may be null if this DbGeometry value does not represent a curve.
        /// </summary>
        public DbGeometry StartPoint
        {
            get { return _spatialProvider.GetStartPoint(this); }
        }

        /// <summary>
        /// Gets a DbGeometry value representing the start point of this value, which may be null if this DbGeometry value does not represent a curve.
        /// </summary>
        public DbGeometry EndPoint
        {
            get { return _spatialProvider.GetEndPoint(this); }
        }

        /// <summary>
        /// Gets a nullable Boolean value indicating whether this DbGeometry value is closed, which may be null if this value does not represent a curve.
        /// </summary>
        public bool? IsClosed
        {
            get { return _spatialProvider.GetIsClosed(this); }
        }

        /// <summary>
        /// Gets a nullable Boolean value indicating whether this DbGeometry value is a ring, which may be null if this value does not represent a curve.
        /// </summary>
        public bool? IsRing
        {
            get { return _spatialProvider.GetIsRing(this); }
        }

        #endregion

        #region LineString, Line, LinearRing

        /// <summary>
        /// Gets the number of points in this DbGeometry value, if it represents a linestring or linear ring.
        /// <returns>The number of elements in this geometry value, if it represents a linestring or linear ring; otherwise <c>null</c>.</returns>
        /// </summary>
        public int? PointCount
        {
            get { return _spatialProvider.GetPointCount(this); }
        }

        /// <summary>
        /// Returns an element of this DbGeometry value from a specific position, if it represents a linestring or linear ring.
        /// <param name="index">The position within this geometry value from which the element should be taken.</param>
        /// <returns>The element in this geometry value at the specified position, if it represents a linestring or linear ring; otherwise <c>null</c>.</returns>
        /// </summary>
        public DbGeometry PointAt(int index)
        {
            return _spatialProvider.PointAt(this, index);
        }

        #endregion

        #region Surface

        /// <summary>
        /// Gets a nullable double value that indicates the area of this DbGeometry value, which may be null if this value does not represent a surface.
        /// </summary>
        public double? Area
        {
            get { return _spatialProvider.GetArea(this); }
        }

        /// <summary>
        /// Gets the DbGeometry value that represents the centroid of this DbGeometry value, which may be null if this value does not represent a surface.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Centroid",
            Justification = "Naming convention prescribed by OGC specification")]
        public DbGeometry Centroid
        {
            get { return _spatialProvider.GetCentroid(this); }
        }

        /// <summary>
        /// Gets a point on the surface of this DbGeometry value, which may be null if this value does not represent a surface.
        /// </summary>
        public DbGeometry PointOnSurface
        {
            get { return _spatialProvider.GetPointOnSurface(this); }
        }

        #endregion

        #region Polygon

        /// <summary>
        /// Gets the DbGeometry value that represents the exterior ring of this DbGeometry value, which may be null if this value does not represent a polygon.
        /// </summary>
        public DbGeometry ExteriorRing
        {
            get { return _spatialProvider.GetExteriorRing(this); }
        }

        /// <summary>
        /// Gets the number of interior rings in this DbGeometry value, if it represents a polygon.
        /// <returns>The number of elements in this geometry value, if it represents a polygon; otherwise <c>null</c>.</returns>
        /// </summary>
        public int? InteriorRingCount
        {
            get { return _spatialProvider.GetInteriorRingCount(this); }
        }

        /// <summary>
        /// Returns an interior ring from this DbGeometry value at a specific position, if it represents a polygon.
        /// <param name="index">The position within this geometry value from which the interior ring should be taken.</param>
        /// <returns>The interior ring in this geometry value at the specified position, if it represents a polygon; otherwise <c>null</c>.</returns>
        /// </summary>
        public DbGeometry InteriorRingAt(int index)
        {
            return _spatialProvider.InteriorRingAt(this, index);
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a string representation of the geometry value.
        /// </summary>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture, "SRID={1};{0}", WellKnownValue.WellKnownText ?? base.ToString(), CoordinateSystemId);
        }

        #endregion
    }
}
