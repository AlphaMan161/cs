// ILSpyBased#2
using UnityEngine;

public class MotorSecurity
{
    private int numMotorSecurityAlerts;

    private Vector3 oldPosition = Vector3.zero;

    private int slopeCounter;

    private float speedYAverage;

    private float speedYCount;

    public MotorSecurity(CharacterMotor motor)
    {
        this.Update(motor);
    }

    public bool Update(CharacterMotor motor)
    {
        Vector3 position = motor.transform.position;
        Vector3 vector = position - this.oldPosition;
        float sqrMagnitude = new Vector3(vector.x, 0f, vector.z).sqrMagnitude;
        float sqrMagnitude2 = vector.sqrMagnitude;
        float num = 3.40282347E+38f;
        if (sqrMagnitude2 == 0f)
        {
            num = 0f;
        }
        else if (sqrMagnitude != 0f)
        {
            num = sqrMagnitude2 / sqrMagnitude;
        }
        if ((double)num > 1.7)
        {
            this.slopeCounter++;
        }
        if (!motor.grounded)
        {
            this.slopeCounter = 0;
        }
        float num2 = position.y - this.oldPosition.y;
        if (num2 > 0f)
        {
            this.speedYAverage += num2;
            this.speedYCount += 1f;
        }
        else
        {
            this.speedYAverage /= this.speedYCount;
            if (this.speedYAverage > 1.5f && this.speedYCount > 25f)
            {
                PlayerManager.Instance.SendEnterBaseRequest(4);
            }
            this.speedYAverage = 0f;
            this.speedYCount = 0f;
        }
        this.oldPosition = position;
        if (this.slopeCounter > 20)
        {
            this.slopeCounter = 0;
            return true;
        }
        return false;
    }

    public int Check(CharacterMotor motor)
    {
        if (this.numMotorSecurityAlerts > 3)
        {
            this.numMotorSecurityAlerts = 0;
            this.slopeCounter = 0;
            return 4;
        }
        return 0;
    }

    public void Alert()
    {
        this.slopeCounter = 0;
        this.numMotorSecurityAlerts++;
    }

    public void ResetAlert()
    {
        this.slopeCounter = 0;
        this.numMotorSecurityAlerts = 0;
    }
}


