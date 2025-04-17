using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpEp_Laser : MonoBehaviour {

    public GameObject HitEffect;
    public GameObject theoreticalEnd;
    public float HitEffectRate=0.1f;
    private float effectTime=0.0f;
    private bool doEffects;  //this is needed because the hit effect needs to be played only if the laser actually hits something, but the muzzzlefire needs to be played all the time
    public int damage = 1;
    public float length = 35.0f;

    public LineRenderer[] myLines;
    public bool uvCorrection;
    public float uvCorrectionLength = 10;
    public float uvScroll = 0;
    private float timeWentX = 0f;

    public GameObject muzzleFire;

    public bool useEffectAlongLine = false;
    public ParticleSystem effectAlongLine;
    public GameObject effectAlongLineGO;
    public float particleDensityPerUnit = 10;

    private float dist=10;

	// Use this for initialization
	void Start ()
    {
        effectTime = HitEffectRate;
	}

    // Update is called once per frame
    void Update()
    {
        effectTime += Time.deltaTime;
        timeWentX += Time.deltaTime * uvScroll;
        if (effectTime > HitEffectRate)
        {
            doEffects = true;
            effectTime -= HitEffectRate;
        }


        if (doEffects&&muzzleFire) Instantiate(muzzleFire, transform.position, transform.parent.transform.rotation); 

            RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, length))
        {

        if (doEffects)
            {
            if (doEffects&&HitEffect) Instantiate(HitEffect, hit.point, Quaternion.identity);
                     
                if (hit.transform.gameObject.GetComponent<SpEp_Enemy>())
                {
                    hit.transform.gameObject.GetComponent<SpEp_Enemy>().HP -= damage;
                    hit.transform.gameObject.GetComponent<SpEp_Enemy>().TakingDamage();
                }

            }

            for (int i = 0; i < myLines.Length; i++)
            {
                myLines[i].SetPosition(1, hit.point);
                myLines[i].SetPosition(0, transform.position);
            }
        }
        else
        {
            for (int i = 0; i < myLines.Length; i++)
            {
                myLines[i].SetPosition(0, transform.position);
                myLines[i].SetPosition(1, theoreticalEnd.transform.position);
            }
        }


       

        if (uvCorrection)
        {
            for (int j = 0; j < myLines.Length; j++)
            {
                dist = Vector3.Distance(myLines[j].GetPosition(0), myLines[j].GetPosition(1));

           
            myLines[j].GetComponent<LineRenderer>().material.SetTextureScale("_MainTex", new Vector2(dist* uvCorrectionLength, 1));
            myLines[j].GetComponent<LineRenderer>().material.SetTextureOffset("_MainTex", new Vector2(timeWentX, 0));
            }
        }

        if (useEffectAlongLine)
        {
            dist = Vector3.Distance(myLines[0].GetPosition(0), myLines[0].GetPosition(1));
            UnityEngine.ParticleSystem.ShapeModule editableShape = effectAlongLine.shape;
            Vector3 newBox = new Vector3(1f, 1f, dist);
            editableShape.scale = newBox;
            effectAlongLine.transform.localPosition = new Vector3(0, 0, dist / 2);
            effectAlongLine.emissionRate = particleDensityPerUnit * dist;

            //effectAlongLine.shape.scale = new Vector3(0.1f, 0.1f, dist);
        }

        doEffects = false;
        
    }
}
