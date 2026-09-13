using System;
using System.Collections.Generic;

namespace RVO;

internal class Agent
{
	internal IList<KeyValuePair<float, Agent>> agentNeighbors_ = new List<KeyValuePair<float, Agent>>();

	internal IList<KeyValuePair<float, Obstacle>> obstacleNeighbors_ = new List<KeyValuePair<float, Obstacle>>();

	internal IList<Line> orcaLines_ = new List<Line>();

	internal Vector2 position_;

	internal Vector2 prefVelocity_;

	internal Vector2 velocity_;

	internal int id_;

	internal int maxNeighbors_;

	internal float maxSpeed_;

	internal float neighborDist_;

	internal float radius_;

	internal float timeHorizon_;

	internal float timeHorizonObst_;

	internal bool needDelete_;

	internal float weight = 1f;

	private Vector2 newVelocity_;

	internal void SetWeight(float targetWeight)
	{
		weight = targetWeight;
	}

	internal float GetWeight()
	{
		return weight;
	}

	internal void computeNeighbors()
	{
		obstacleNeighbors_.Clear();
		float rangeSq = RVOMath.sqr(timeHorizonObst_ * maxSpeed_ + radius_);
		Simulator.Instance.kdTree_.computeObstacleNeighbors(this, rangeSq);
		agentNeighbors_.Clear();
		if (maxNeighbors_ > 0)
		{
			rangeSq = RVOMath.sqr(neighborDist_);
			Simulator.Instance.kdTree_.computeAgentNeighbors(this, ref rangeSq);
		}
	}

	internal void computeNewVelocity()
	{
		orcaLines_.Clear();
		float num = 1f / timeHorizonObst_;
		Line item = default(Line);
		for (int i = 0; i < obstacleNeighbors_.Count; i++)
		{
			Obstacle obstacle = obstacleNeighbors_[i].Value;
			Obstacle obstacle2 = obstacle.next_;
			Vector2 vector = obstacle.point_ - position_;
			Vector2 vector2 = obstacle2.point_ - position_;
			bool flag = false;
			for (int j = 0; j < orcaLines_.Count; j++)
			{
				if (RVOMath.det(num * vector - orcaLines_[j].point, orcaLines_[j].direction) - num * radius_ >= -1E-05f && RVOMath.det(num * vector2 - orcaLines_[j].point, orcaLines_[j].direction) - num * radius_ >= -1E-05f)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				continue;
			}
			float num2 = RVOMath.absSq(vector);
			float num3 = RVOMath.absSq(vector2);
			float num4 = RVOMath.sqr(radius_);
			Vector2 vector3 = obstacle2.point_ - obstacle.point_;
			float num5 = -vector * vector3 / RVOMath.absSq(vector3);
			float num6 = RVOMath.absSq(-vector - num5 * vector3);
			if (num5 < 0f && num2 <= num4)
			{
				if (obstacle.convex_)
				{
					item.point = new Vector2(0f, 0f);
					item.direction = RVOMath.normalize(new Vector2(0f - vector.y(), vector.x()));
					orcaLines_.Add(item);
				}
				continue;
			}
			if (num5 > 1f && num3 <= num4)
			{
				if (obstacle2.convex_ && RVOMath.det(vector2, obstacle2.direction_) >= 0f)
				{
					item.point = new Vector2(0f, 0f);
					item.direction = RVOMath.normalize(new Vector2(0f - vector2.y(), vector2.x()));
					orcaLines_.Add(item);
				}
				continue;
			}
			if (num5 >= 0f && num5 < 1f && num6 <= num4)
			{
				item.point = new Vector2(0f, 0f);
				item.direction = -obstacle.direction_;
				orcaLines_.Add(item);
				continue;
			}
			Vector2 vector4;
			Vector2 vector5;
			if (num5 < 0f && num6 <= num4)
			{
				if (!obstacle.convex_)
				{
					continue;
				}
				obstacle2 = obstacle;
				float num7 = RVOMath.sqrt(num2 - num4);
				vector4 = new Vector2(vector.x() * num7 - vector.y() * radius_, vector.x() * radius_ + vector.y() * num7) / num2;
				vector5 = new Vector2(vector.x() * num7 + vector.y() * radius_, (0f - vector.x()) * radius_ + vector.y() * num7) / num2;
			}
			else if (num5 > 1f && num6 <= num4)
			{
				if (!obstacle2.convex_)
				{
					continue;
				}
				obstacle = obstacle2;
				float num8 = RVOMath.sqrt(num3 - num4);
				vector4 = new Vector2(vector2.x() * num8 - vector2.y() * radius_, vector2.x() * radius_ + vector2.y() * num8) / num3;
				vector5 = new Vector2(vector2.x() * num8 + vector2.y() * radius_, (0f - vector2.x()) * radius_ + vector2.y() * num8) / num3;
			}
			else
			{
				if (obstacle.convex_)
				{
					float num9 = RVOMath.sqrt(num2 - num4);
					vector4 = new Vector2(vector.x() * num9 - vector.y() * radius_, vector.x() * radius_ + vector.y() * num9) / num2;
				}
				else
				{
					vector4 = -obstacle.direction_;
				}
				if (obstacle2.convex_)
				{
					float num10 = RVOMath.sqrt(num3 - num4);
					vector5 = new Vector2(vector2.x() * num10 + vector2.y() * radius_, (0f - vector2.x()) * radius_ + vector2.y() * num10) / num3;
				}
				else
				{
					vector5 = obstacle.direction_;
				}
			}
			Obstacle previous_ = obstacle.previous_;
			bool flag2 = false;
			bool flag3 = false;
			if (obstacle.convex_ && RVOMath.det(vector4, -previous_.direction_) >= 0f)
			{
				vector4 = -previous_.direction_;
				flag2 = true;
			}
			if (obstacle2.convex_ && RVOMath.det(vector5, obstacle2.direction_) <= 0f)
			{
				vector5 = obstacle2.direction_;
				flag3 = true;
			}
			Vector2 vector6 = num * (obstacle.point_ - position_);
			Vector2 vector7 = num * (obstacle2.point_ - position_);
			Vector2 vector8 = vector7 - vector6;
			float num11 = ((obstacle == obstacle2) ? 0.5f : ((velocity_ - vector6) * vector8 / RVOMath.absSq(vector8)));
			float num12 = (velocity_ - vector6) * vector4;
			float num13 = (velocity_ - vector7) * vector5;
			if ((num11 < 0f && num12 < 0f) || (obstacle == obstacle2 && num12 < 0f && num13 < 0f))
			{
				Vector2 vector9 = RVOMath.normalize(velocity_ - vector6);
				item.direction = new Vector2(vector9.y(), 0f - vector9.x());
				item.point = vector6 + radius_ * num * vector9;
				orcaLines_.Add(item);
				continue;
			}
			if (num11 > 1f && num13 < 0f)
			{
				Vector2 vector10 = RVOMath.normalize(velocity_ - vector7);
				item.direction = new Vector2(vector10.y(), 0f - vector10.x());
				item.point = vector7 + radius_ * num * vector10;
				orcaLines_.Add(item);
				continue;
			}
			float num14 = ((num11 < 0f || num11 > 1f || obstacle == obstacle2) ? float.PositiveInfinity : RVOMath.absSq(velocity_ - (vector6 + num11 * vector8)));
			float num15 = ((num12 < 0f) ? float.PositiveInfinity : RVOMath.absSq(velocity_ - (vector6 + num12 * vector4)));
			float num16 = ((num13 < 0f) ? float.PositiveInfinity : RVOMath.absSq(velocity_ - (vector7 + num13 * vector5)));
			if (num14 <= num15 && num14 <= num16)
			{
				item.direction = -obstacle.direction_;
				item.point = vector6 + radius_ * num * new Vector2(0f - item.direction.y(), item.direction.x());
				orcaLines_.Add(item);
			}
			else if (num15 <= num16)
			{
				if (!flag2)
				{
					item.direction = vector4;
					item.point = vector6 + radius_ * num * new Vector2(0f - item.direction.y(), item.direction.x());
					orcaLines_.Add(item);
				}
			}
			else if (!flag3)
			{
				item.direction = -vector5;
				item.point = vector7 + radius_ * num * new Vector2(0f - item.direction.y(), item.direction.x());
				orcaLines_.Add(item);
			}
		}
		int count = orcaLines_.Count;
		float num17 = 1f / timeHorizon_;
		Line item2 = default(Line);
		for (int k = 0; k < agentNeighbors_.Count; k++)
		{
			Agent value = agentNeighbors_[k].Value;
			Vector2 vector11 = value.position_ - position_;
			Vector2 vector12 = velocity_ - value.velocity_;
			float num18 = RVOMath.absSq(vector11);
			float num19 = radius_ + value.radius_;
			float num20 = RVOMath.sqr(num19);
			Vector2 vector15;
			if (num18 > num20)
			{
				Vector2 vector13 = vector12 - num17 * vector11;
				float num21 = RVOMath.absSq(vector13);
				float num22 = vector13 * vector11;
				if (num22 < 0f && RVOMath.sqr(num22) > num20 * num21)
				{
					float num23 = RVOMath.sqrt(num21);
					Vector2 vector14 = vector13 / num23;
					item2.direction = new Vector2(vector14.y(), 0f - vector14.x());
					vector15 = (num19 * num17 - num23) * vector14;
				}
				else
				{
					float num24 = RVOMath.sqrt(num18 - num20);
					if (RVOMath.det(vector11, vector13) > 0f)
					{
						item2.direction = new Vector2(vector11.x() * num24 - vector11.y() * num19, vector11.x() * num19 + vector11.y() * num24) / num18;
					}
					else
					{
						item2.direction = -new Vector2(vector11.x() * num24 + vector11.y() * num19, (0f - vector11.x()) * num19 + vector11.y() * num24) / num18;
					}
					vector15 = vector12 * item2.direction * item2.direction - vector12;
				}
			}
			else
			{
				float num25 = 1f / Simulator.Instance.timeStep_;
				Vector2 vector16 = vector12 - num25 * vector11;
				float num26 = RVOMath.abs(vector16);
				Vector2 vector17 = ((!(num26 < float.Epsilon)) ? (vector16 / num26) : new Vector2(0f, 0f));
				item2.direction = new Vector2(vector17.y(), 0f - vector17.x());
				vector15 = (num19 * num25 - num26) * vector17;
			}
			float num27 = GetWeight();
			float num28 = value.GetWeight();
			float num29 = num28 / (num27 + num28);
			item2.point = velocity_ + num29 * vector15;
			orcaLines_.Add(item2);
		}
		int num30 = linearProgram2(orcaLines_, maxSpeed_, prefVelocity_, directionOpt: false, ref newVelocity_);
		if (num30 < orcaLines_.Count)
		{
			linearProgram3(orcaLines_, count, num30, maxSpeed_, ref newVelocity_);
		}
	}

	internal void insertAgentNeighbor(Agent agent, ref float rangeSq)
	{
		if (this == agent)
		{
			return;
		}
		float num = RVOMath.absSq(position_ - agent.position_);
		if (num < rangeSq)
		{
			if (agentNeighbors_.Count < maxNeighbors_)
			{
				agentNeighbors_.Add(new KeyValuePair<float, Agent>(num, agent));
			}
			int num2 = agentNeighbors_.Count - 1;
			while (num2 != 0 && num < agentNeighbors_[num2 - 1].Key)
			{
				agentNeighbors_[num2] = agentNeighbors_[num2 - 1];
				num2--;
			}
			agentNeighbors_[num2] = new KeyValuePair<float, Agent>(num, agent);
			if (agentNeighbors_.Count == maxNeighbors_)
			{
				rangeSq = agentNeighbors_[agentNeighbors_.Count - 1].Key;
			}
		}
	}

	internal void insertObstacleNeighbor(Obstacle obstacle, float rangeSq)
	{
		Obstacle next_ = obstacle.next_;
		float num = RVOMath.distSqPointLineSegment(obstacle.point_, next_.point_, position_);
		if (num < rangeSq)
		{
			obstacleNeighbors_.Add(new KeyValuePair<float, Obstacle>(num, obstacle));
			int num2 = obstacleNeighbors_.Count - 1;
			while (num2 != 0 && num < obstacleNeighbors_[num2 - 1].Key)
			{
				obstacleNeighbors_[num2] = obstacleNeighbors_[num2 - 1];
				num2--;
			}
			obstacleNeighbors_[num2] = new KeyValuePair<float, Obstacle>(num, obstacle);
		}
	}

	internal void update()
	{
		velocity_ = newVelocity_;
		position_ += velocity_ * Simulator.Instance.timeStep_;
	}

	private bool linearProgram1(IList<Line> lines, int lineNo, float radius, Vector2 optVelocity, bool directionOpt, ref Vector2 result)
	{
		float num = lines[lineNo].point * lines[lineNo].direction;
		float num2 = RVOMath.sqr(num) + RVOMath.sqr(radius) - RVOMath.absSq(lines[lineNo].point);
		if (num2 < 0f)
		{
			return false;
		}
		float num3 = RVOMath.sqrt(num2);
		float num4 = 0f - num - num3;
		float num5 = 0f - num + num3;
		for (int i = 0; i < lineNo; i++)
		{
			float num6 = RVOMath.det(lines[lineNo].direction, lines[i].direction);
			float num7 = RVOMath.det(lines[i].direction, lines[lineNo].point - lines[i].point);
			if (RVOMath.fabs(num6) <= 1E-05f)
			{
				if (num7 < 0f)
				{
					return false;
				}
				continue;
			}
			float val = num7 / num6;
			if (num6 >= 0f)
			{
				num5 = Math.Min(num5, val);
			}
			else
			{
				num4 = Math.Max(num4, val);
			}
			if (num4 > num5)
			{
				return false;
			}
		}
		if (directionOpt)
		{
			if (optVelocity * lines[lineNo].direction > 0f)
			{
				result = lines[lineNo].point + num5 * lines[lineNo].direction;
			}
			else
			{
				result = lines[lineNo].point + num4 * lines[lineNo].direction;
			}
		}
		else
		{
			float num8 = lines[lineNo].direction * (optVelocity - lines[lineNo].point);
			if (num8 < num4)
			{
				result = lines[lineNo].point + num4 * lines[lineNo].direction;
			}
			else if (num8 > num5)
			{
				result = lines[lineNo].point + num5 * lines[lineNo].direction;
			}
			else
			{
				result = lines[lineNo].point + num8 * lines[lineNo].direction;
			}
		}
		return true;
	}

	private int linearProgram2(IList<Line> lines, float radius, Vector2 optVelocity, bool directionOpt, ref Vector2 result)
	{
		if (directionOpt)
		{
			result = optVelocity * radius;
		}
		else if (RVOMath.absSq(optVelocity) > RVOMath.sqr(radius))
		{
			result = RVOMath.normalize(optVelocity) * radius;
		}
		else
		{
			result = optVelocity;
		}
		for (int i = 0; i < lines.Count; i++)
		{
			if (RVOMath.det(lines[i].direction, lines[i].point - result) > 0f)
			{
				Vector2 vector = result;
				if (!linearProgram1(lines, i, radius, optVelocity, directionOpt, ref result))
				{
					result = vector;
					return i;
				}
			}
		}
		return lines.Count;
	}

	private void linearProgram3(IList<Line> lines, int numObstLines, int beginLine, float radius, ref Vector2 result)
	{
		float num = 0f;
		Line item = default(Line);
		for (int i = beginLine; i < lines.Count; i++)
		{
			if (!(RVOMath.det(lines[i].direction, lines[i].point - result) > num))
			{
				continue;
			}
			IList<Line> list = new List<Line>();
			for (int j = 0; j < numObstLines; j++)
			{
				list.Add(lines[j]);
			}
			for (int k = numObstLines; k < i; k++)
			{
				float num2 = RVOMath.det(lines[i].direction, lines[k].direction);
				if (RVOMath.fabs(num2) <= 1E-05f)
				{
					if (lines[i].direction * lines[k].direction > 0f)
					{
						continue;
					}
					item.point = 0.5f * (lines[i].point + lines[k].point);
				}
				else
				{
					item.point = lines[i].point + RVOMath.det(lines[k].direction, lines[i].point - lines[k].point) / num2 * lines[i].direction;
				}
				item.direction = RVOMath.normalize(lines[k].direction - lines[i].direction);
				list.Add(item);
			}
			Vector2 vector = result;
			if (linearProgram2(list, radius, new Vector2(0f - lines[i].direction.y(), lines[i].direction.x()), directionOpt: true, ref result) < list.Count)
			{
				result = vector;
			}
			num = RVOMath.det(lines[i].direction, lines[i].point - result);
		}
	}
}
