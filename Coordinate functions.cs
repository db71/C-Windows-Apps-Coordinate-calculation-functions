		double EarthRadius = 6371000D;
		
		public double CalculateBearing(double lat1, double lon1, double lat2, double lon2)
		{
			try
			{
				double dLon = ToRad(lon2 - lon1);
				double dPhi = Math.Log(Math.Tan(ToRad(lat2) / 2D + Math.PI / 4D) / Math.Tan(ToRad(lat1) / 2D + Math.PI / 4D));
				if (Math.Abs(dLon) > Math.PI)
				{
					dLon = dLon > 0D ? -(2D * Math.PI - dLon) : (2D * Math.PI + dLon);
				}
				return ToBearing(Math.Atan2(dLon, dPhi));
			}
			catch (Exception ex)
			{
				BugReport(1, ex);
				return 0;
			}
		}

		public double CalculateDistance(double Lat1, double Lon1, double Lat2, double Lon2)
		{
			try
			{
				// convert latitude and longitude values to radians
				double prevRadLat = ToRad(Lat1);
				double prevRadLong = ToRad(Lon1);
				double currRadLat = ToRad(Lat2);
				double currRadLong = ToRad(Lon2);

				// calculate radian delta between each position.
				double radDeltaLat = currRadLat - prevRadLat;
				double radDeltaLong = currRadLong - prevRadLong;

				// calculate distance
				double expr1 = (Math.Sin(radDeltaLat / 2D) *
							 Math.Sin(radDeltaLat / 2D)) +
							(Math.Cos(prevRadLat) *
							 Math.Cos(currRadLat) *
							 Math.Sin(radDeltaLong / 2D) *
							 Math.Sin(radDeltaLong / 2D));
				double expr2 = 2D * Math.Atan2(Math.Sqrt(expr1),
											 Math.Sqrt(1D - expr1));

				double distance = (EarthRadius * expr2);
				return distance;  // return results as meters
			}
			catch (Exception ex)
			{
				BugReport(1, ex);
				return 0;
			}
		}

		public List<BasicGeoposition> CalculateCircle(BasicGeoposition Position, double Radius)
		{
			try
			{
				List<BasicGeoposition> GeoPositions = new List<BasicGeoposition>();
				for (int i = 0; i <= 360; i++)
				{
					double Circumference = 2D * Math.PI * EarthRadius;
					double Bearing = ToRad(i);
					double CircumferenceLatitudeCorrected = 2D * Math.PI * Math.Cos(ToRad(Position.Latitude)) * EarthRadius;
					double lat1 = Circumference / 360D * Position.Latitude;
					double lon1 = CircumferenceLatitudeCorrected / 360D * Position.Longitude;
					double lat2 = lat1 + Math.Sin(Bearing) * Radius;
					double lon2 = lon1 + Math.Cos(Bearing) * Radius;
					BasicGeoposition NewBasicPosition = new BasicGeoposition();
					NewBasicPosition.Latitude = lat2 / (Circumference / 360D);
					NewBasicPosition.Longitude = lon2 / (CircumferenceLatitudeCorrected / 360D);
					GeoPositions.Add(NewBasicPosition);
				}
				return GeoPositions;
			}
			catch (Exception ex)
			{
				BugReport(1, ex);
				return null;
			}
		}

		private double ToRad(double degrees)
		{
			try
			{
				return degrees * (Math.PI / 180D);
			}
			catch (Exception ex)
			{
				BugReport(1, ex);
				return 0;
			}
		}

		private double ToDegrees(double radians)
		{
			try
			{
				return radians * 180D / Math.PI;
			}
			catch (Exception ex)
			{
				BugReport(1, ex);
				return 0;
			}
		}

		private double ToBearing(double radians)
		{
			try
			{
				return (ToDegrees(radians) + 360D) % 360D;
			}
			catch (Exception ex)
			{
				BugReport(1, ex);
				return 0;
			}
		}

